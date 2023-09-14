/**
* Name: Water flowing in the red river bed
* Author: drogoul
* Tags: 
*/
model Terrain

global parent: physical_world {
//	bool use_native <- true;
	bool accurate_collision_detection <- true;
	// We scale the DEM up a little
	//	float z_scale <-  0.5;
	float g_scale <- 0.05;
	float step <- 1.0 / 120;
	//	bool flowing <- true;
	point gravity <- {0, 0, -9.81};
	int number_of_water_units <- 1 min: 0 max: 10;
	list<point> origins_of_flow <- [{world.shape.width / 2, world.shape.height / 2}];
//		field terrain <- field(grid_file("../includes/t.tiff"));
	int num <-20;
	field terrain <- field(num, num);

	//	geometry shape <- box({terrain.columns, terrain.rows, max(terrain.bands[0])*z_scale});
	geometry shape <- box({num, num, 0});
	float restitution <- 0.8; // the "bounciness" of the world
	float friction <- 0.2; // the deceleration it imposes on other objects
	int h1 <- 3;
	int h2 <- 2;
	float diffusion_rate <- 0.6;

	init {
		do register([self]);
		geometry box <- box(num + 3, 3, 10);
		create wall from: [box at_location ({num / 2, 0}), box rotated_by 90 at_location ({0, num / 2}), box at_location ({num / 2, num}), box rotated_by 90 at_location
		({num, num / 2})];
		loop zz from: 0 to: h1 {
			loop i from: 2 to: num - 2 {
				loop j from: 2 to: num - 2 {
					create water with: [location::{i, j, zz}];
				}

			}

		}
		//	loop zz from:0 to:9{
		//		loop i from:18 to:22{
		//			loop j from:18 to:22{
		//				create water with: [location::{i,j,zz}];
		//			}
		//		}
		//		
		//	}
		loop times: 3 {
			int xx <- 3 + rnd(num - 5);
			int yy <- 3 + rnd(num - 5);
			create building with: [location: {xx, yy, h1 + 1 + h2 * 0}];
			create building with: [location: {xx, yy, h1 + 1 + h2 * 1}];
			create building with: [location: {xx, yy, h1 + 1 + h2 * 2}];
			create building with: [location: {xx, yy, h1 + 1 + h2 * 3}];
		}

		ask water {
			neighbour_cells <- (self neighbors_at 1);
		}

		source <- any(water);
	}

	water source;

//	reflex adding_input_water {
//		float water_input <- 5.0;//rnd(100) / 100;
//		ask source {
//			water_height <- water_height + water_input;
//		}
//
//	}
//
//	//Reflex to update the color of the cell
//	reflex update_cell_color {
//		ask water {
//			do update_color;
////			if(water_level>1){do die;}
//		}
//
//	} 
	reflex flow1 {
		ask building{
			
	 restitution <- 0.5;
	 friction <- 0.5;
	 damping <- 0.5;
	 angular_damping <- 1.0;
		}
//		ask (water sort_by ((each.altitude + each.water_height))) {
//			already <- false;
//			do flow;
//		}
				ask number_of_water_units among (water) {
					do die;
				}
		//			loop origin_of_flow over: origins_of_flow {
		//				int x <- int(min(terrain.columns - 1, max(0, origin_of_flow.x + rnd(10) - 5)));
		//				int y <- int(min(terrain.rows - 1, max(0, origin_of_flow.y + rnd(10) - 5)));
		//				point p <- origin_of_flow + {rnd(10) - 5, rnd(10 - 5), terrain.bands[0][x, y] + 4};
		//				create water number: number_of_water_units with: [location::p];
		//			}
	}
	}

species wall skills: [static_body] {
	float restitution <- 0.0;
	float friction <- 0.0;

	aspect default {
		draw shape wireframe: true;
	}

}

species building skills: [dynamic_body] {

	init {
//		velocity <- {rnd(2) - 1, rnd(2) - 1, rnd(2) - 1};
	}

	geometry shape <- box(2, 2, h2);
	float mass <- 10.05;
	float restitution <- 0.0;
	float friction <- 0.0;
	float damping <- 0.0;
	float angular_damping <- 0.0;
	rgb color <- #red;
}

species water skills: [dynamic_body] {
	geometry shape <- cube(1.0);
	float restitution <- 0.5;
	float friction <- 0.5;
	float damping <- 0.5;
	float angular_damping <- 0.5;
	float mass <- 10.005;
	rgb color <- one_of(brewer_colors("Blues"));
	float water_level;
	float water_height <- 0.0 min: 0.0;
	float height;
	float altitude;
	bool already <- false;
	list<water> neighbour_cells;

	action flow {
	//We get all the cells already done
		list<water> neighbour_cells_al <- neighbour_cells where (each.already);
		//If there are cells already done then we continue
		if (!empty(neighbour_cells_al)) {
		//We compute the height of the neighbours cells according to their altitude, water_height and obstacle_height
			ask neighbour_cells_al {
				height <- altitude + water_height;
			}
			//The height of the cell is equals to its altitude and water height
			height <- altitude + water_height;
			//The water of the cells will flow to the neighbour cells which have a height less than the height of the actual cell
			list<water> flow_cells <- (neighbour_cells_al); // where (height > each.height)) ;
			//If there are cells, we compute the water flowing
			if (!empty(flow_cells)) {
				loop flow_cell over: shuffle(flow_cells) sort_by (each.height) {
					float water_flowing <- max([0.0, min([(height - flow_cell.height), water_height * diffusion_rate])]);
					water_height <- water_height - water_flowing;
					flow_cell.water_height <- flow_cell.water_height + water_flowing;
					height <- altitude + water_height;
				}

			}

		}

		already <- true;
	}

	action update_color {
         int val_water <- 0;
         val_water <- max([0, min([255, int(255 * (1 - (water_height / 12.0)))])]) ;  
         color <- rgb([val_water, val_water, 255]);
		water_level <- water_height + altitude;
	}

	reflex manage_location when: location.z < -1 {
		do die;
	}

	aspect default {
	//		if (location.y > 10){
	//		}
		draw shape color: color;
	}

}

experiment "3D view" type: gui {

//	string camera_loc <- #from_up_front;
//	int distance <- 200;

//	action _init_ {
//		create simulation with: [z_scale::0.3];
//		create simulation with: [z_scale::1.0];
//		create simulation with: [z_scale::2.0];
//		create simulation with: [z_scale::3.0];
//	} 
//	parameter "Location of the camera" var: camera_loc among: [#from_up_front, #from_above, #from_up_left, #from_up_right];
//	parameter "Distance of the camera" var: distance min: 1 max: 1000 slider: true;
//	parameter "Number of water agents per cycle" var: number_of_water_units;
	output synchronized: false {
	//		layout #split;
		display "Flow" type: 3d background: #white antialias: true {
		//			camera #default location: camera_loc distance: distance dynamic: true;
		//			graphics world {
		//				draw "Scale: " + z_scale color: #cadetblue font: font("Helvetica", 18, #bold) at: {world.location.x, -10, 25} anchor: #center depth: 2 rotate: -90::{1,0,0};
		//				draw aabb wireframe: true color: #lightblue;
		//			}
		//						mesh terrain triangulation: true scale:z_scale-10 refresh: false smooth:true no_data:0.0	  ;

		//			mesh terrain triangulation: true refresh: false scale: z_scale smooth: 2;
			species wall;
			species water;
			species building;
			//			event #mouse_down {
			//				point p <- #user_location;
			//				origins_of_flow << {p.x, p.y};
			//			}
		}

	}

}
	