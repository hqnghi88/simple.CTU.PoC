/***
* Name: Water flow in a river graph, using water units
* Author: Benoit Gaudou
* Description: In this model, the flow of water is simulated through the move of water units from a source point toward the outlet point.
* 	Every step a source point is chosen to create a water unit, that will then flow toward the outlet point.
* Tags: shapefile, gis, graph, gui, hydrology, water flow
***/

model Waterflowriverwaterunit
import "../simple.template.universe/Template GAMA model/models/UnityLink.gaml"

global {
	file river_shape_file <- shape_file("../includes/rivers.shp");
	file poi_file <- shape_file("../includes/poi.shp");

	geometry shape <- envelope(river_shape_file) + 500;
	
	graph the_river;

	init {
		create river from: river_shape_file;
		create poi from: poi_file; 
		do add_background_data(poi collect (each.shape buffer 200.0), 0.01, false);
		do add_background_data(river collect (each.shape buffer 200.0), 0.01, false);
		
		the_river <- as_edge_graph(river);
	}
	
	reflex c_water when: flip(0.01) {
		create water { 
			location <- one_of(poi where (each.type = "source")).location;
			target <- one_of(poi where (each.type = "outlet")) ;
		}
		
		agents_to_send <- list(water);
	}
}

species poi {
	string type;
	
	aspect default {
		draw circle(500) color: (type="source") ? #green : #red border: #black;		
	}	
}

species river {
	aspect default {
		draw shape + 30 color: #blue;		
	}
}

species water skills: [moving] parent: agent_to_send{
	poi target ;

	reflex move {
		do goto target: target on: the_river speed: 100.0;
	}	
	
	aspect default {
		draw circle(500) color: #blue border: #black;
	}
}

experiment flow type: gui parent: vr_xp{
	output {
	 	display "Water Unit" { 
			species river ; 
			species water;	
			species poi;			
		}
	}
}
