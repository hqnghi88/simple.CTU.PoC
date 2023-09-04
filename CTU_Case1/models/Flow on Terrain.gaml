/**
* Name: Water flowing in the red river bed
* Author: drogoul
* Tags: 
*/


model Terrain

global parent: physical_world {
	bool use_native <- true;
	bool accurate_collision_detection <- true;
	// We scale the DEM up a little
	float z_scale <-  20.5;
	float g_scale <-  0.05;
	float step <-  1.0/120;	
//	bool flowing <- true;
	point gravity <- {-g_scale,g_scale, -9.81};
	int number_of_water_units <- 1 min: 0 max: 10;
	list<point> origins_of_flow <- [{world.shape.width/2,world.shape.height/2} ];
	field terrain <- field(grid_file("../includes/t.tiff"));

	geometry shape <- box({terrain.columns, terrain.rows, max(terrain.bands[0])*z_scale});
	float friction <- 0.0;
	float restitution <- 0.5;


	init {
		do register([self]);
	}

	reflex flow {
			loop origin_of_flow over: origins_of_flow {
				int x <- int(min(terrain.columns - 1, max(0, origin_of_flow.x + rnd(10) - 5)));
				int y <- int(min(terrain.rows - 1, max(0, origin_of_flow.y + rnd(10) - 5)));
				point p <- origin_of_flow + {rnd(10) - 5, rnd(10 - 5), terrain.bands[0][x, y] + 104};
				create water number: number_of_water_units with: [location::p];
			}
	}
}

species water skills: [dynamic_body] {
	geometry shape <- cube(5.0);
	float friction <- 0.5;
	float damping <- 0.0;
	float mass <- 0.005;
	float restitution<-1.0;
	rgb color <- one_of(brewer_colors("Blues"));
	

	aspect default {
		if (location.y > 10){
		draw shape color: color;}
	}
	
		
	reflex manage_location when: location.z < 0 {
		do die;
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
 	parameter "Number of water agents per cycle" var: number_of_water_units; 
	
	output {
//		layout #split;
		display "Flow" type: 3d background: #white   antialias: true {
//			camera #default location: camera_loc distance: distance dynamic: true;
//			graphics world {
//				draw "Scale: " + z_scale color: #cadetblue font: font("Helvetica", 18, #bold) at: {world.location.x, -10, 25} anchor: #center depth: 2 rotate: -90::{1,0,0};
//				draw aabb wireframe: true color: #lightblue;
//			}
						mesh terrain triangulation: true scale:z_scale-10 refresh: false smooth:true no_data:0.0	  ;
			
//			mesh terrain triangulation: true refresh: false scale: z_scale smooth: 2;
			species water;
//			event #mouse_down {
//				point p <- #user_location;
//				origins_of_flow << {p.x, p.y};
//			}
		}

	}}
	