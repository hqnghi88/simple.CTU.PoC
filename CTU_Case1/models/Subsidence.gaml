model Subsidence
/**
 * The model is inheriting from 'physical_world' a special model species that provides access to the physics engine -- and the possibility
 * to manage physical agents. In this model, the world itself is not a physical agent
 */
global parent: physical_world {
	bool use_native <- true;
	//Step (in #sec) passed to the physics engine. The same step is used for the simulation and the physics engine. The accuracy and synchronization
	//between the two can be controlled by max_substeps. A too large step (e.g. 1#sec) would make the lava 'pass through' the ground (tunnel effect).
	//A too small (e.g. 0.01), while more accurate, would, given the velocity of the lava, slow everything down on useless computations.
	float step <- 0.05;
	//A boolean that controls whether or not the lava will erode the ground
	bool erosion;
	float uncertainty -> {rnd(10.0) - 5};
	// Support for display parameters
	bool show_legend;
	bool draw_inside;
	patches highest;

	init {
		highest <- patches with_max_of each.grid_value;
	}
	//Every step the world creates a lava agent near the top of the highest patch in the terrain. It is provided with an initial high vertical velocity.
	reflex flow {
		ask highest {
			create lava number: 1 {
				location <- {highest.location.x + uncertainty, highest.location.y + uncertainty, 4};
				velocity <- velocity + {0, 0, 2 +rnd(6 )  };
			}

		}

	}

}

/**
 * The patches come as a grid created after a simple DEM file and each cell is provided with a 'static body' in the physical world. The whole grid represents
 * an approximate terrain (or heightmap). Since the patches are agents, they can individually respond to events or have their own behavior, making the whole 
 * a powerful way to describe dynamic environments.
 */
grid patches file: grid_file("../includes/DEM/mnt50.asc") parallel:true skills: [static_body] {
	float friction <- 0.5;
	float restitution <- 0.2;

	init {
		grid_value <- 3  -grid_value / 1  ;
	}
	//This action is a 'callback' action, one of the two (with 'contact_removed_with') called by the physics engine when a contact occurs between two agents. 
	// When redefined, it allows agents to react to contacts. Here, every new contact with a lava agent makes a patch decrease its height (grid_value) 
	// and that of its neigbors by a small amount, as well as stop the lava agent quite brutally (clearing all the forces applied to it) to imitate "stickiness"
	action contact_added_with (agent other) {
		if (erosion) {
			grid_value <- max([0,grid_value - 0.01]);
			ask neighbors {
				grid_value <- max([0,grid_value - 0.005]);
				do update_body;
			}

			do update_body;
		}

	}

	aspect default {
		if (draw_inside) {
			draw aabb wireframe: true border: #white;
		}

	}

}

/**
 * Species that represents the lava erupting from the vulcano. Their physical body will be a sphere, weighting 4#kg, offering no restitution but a lot of friction.
 */
species lava skills: [dynamic_body] {
	geometry shape <- cube(2.75);
	float mass <- 1.0;
	rgb color <- one_of(brewer_colors("Reds"));
	float restitution <- 0.2;
	float friction <- 0.3;
	float damping <- 0.1;
	float angular_damping <- 0.1;

	//When a lava agent falls from the edges of the world, it is removed from the simulation (and the physical world as well).		
	reflex manage_location when: location.z < -20 {
		do die;
	}

	aspect default {
		draw shape color: color;
		if (draw_inside) {
			draw aabb color: #lightblue wireframe: true;
			draw line(location, location + velocity) color: #yellow end_arrow: 1 width: 1;
		}

	}

}

experiment "3D view" type: gui {
	font title <- font("Helvetica", 12, #bold);
	parameter "Show inside structures (velocities and aabbs)" var: draw_inside <- false;
	parameter "Better collision detection" var: accurate_collision_detection <- true;
	parameter "Enable erosion" var: erosion <- true;
	parameter "Show legend" var: show_legend <- true;
	output {
		display "3D" type: 3d axes: false background: #black camera: #from_up_front antialias: true { 
			graphics world {
				draw world depth: 0.5 texture: ("../includes/water.gif");
			}
			//The terrain is represented as a field (but could be equally represented as a grid		
			mesh patches grayscale:true triangulation: true smooth: true;
			//We add to the representation the individual 'aabb's (axis-aligned bounding boxes) of the patches if 'draw_inside' is true
//			species patches;
			//Finally, each lava agent is represented (with its velocity if 'draw_inside' is true)
			species lava transparency: 0.9;
		}

	}

}