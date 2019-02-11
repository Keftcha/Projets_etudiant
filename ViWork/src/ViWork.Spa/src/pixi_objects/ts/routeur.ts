//import {all} from "pixi.js"


import * as PIXI from "pixi.js"
import {Routeur} from "../../objects/ts/routeur";
import { Idrawable } from './Idrawable';
import { pixi_Port} from "./port";
import { pixi_NetWorkCard } from "./networkcard";

export class pixi_Router implements Idrawable{
    material: Routeur;
    texture: PIXI.Texture;
    sprite_path: string;
    container: PIXI.Container;
    sprite: PIXI.Sprite;
    renderer: any;
    NwCart: any;
    positionX: any;
    positionY: any;

    constructor() {
        this.container = new PIXI.Container();
        this.NwCart = [];
        this.sprite_path = process.env.VUE_APP_BACKEND+"/images/icons/router.png";       
    }

    take(positionX: number , positionY: number ) {
       
 
        // the computer change his sprite and his position is under the pointer while he don't put ip
    }

    put() {
        // put the computer at his new position (fixed position), don't forget to change his sprite
    }

    draw(container: PIXI.Container, renderer:any) {

        this.renderer = renderer
        const sprite = PIXI.Sprite.fromImage(this.sprite_path)
       
        
        console.log(sprite)
        sprite.anchor.x = 0;
        sprite.anchor.y = 0;

        sprite.width = 100;
        sprite.height =100;
     
        sprite.x =container.position.x/2;
        sprite.y = container.position.y/2;
    
        this.sprite = sprite;

        this.container.addChild(sprite);

        for (var i=0; i < this.material.network_cards.length; i++ ){
            this.CreatePort(this.container,60+(20*i)-(sprite.width/2),20+(sprite.height/2), i);
        }
        
        container.addChild(this.container)

        function animate(){      
            requestAnimationFrame(animate);
            renderer.render(container);           
        }
        animate();
        
    }
    SetMaterial(nbCard){
        this.material = new Routeur(nbCard)
    }

    Move(material: pixi_Router){

        this.container.interactive = true;
        this.container.buttonMode = true;
        this.container.on('mousedown', onDragStart)
		    .on('touchstart', onDragStart)
		    .on('mouseup', onDragEnd)
		    .on('mouseupoutside', onDragEnd)
		    .on('touchend', onDragEnd)
		    .on('touchendoutside', onDragEnd)
		    .on('mousemove', onDragMove)
            .on('touchmove', onDragMove);


            function onDragStart(event) {
                // store a reference to the data
                    this.data = event.data;
                    this.alpha = 0.5;
                    this.dragging = true;
                    this.dragPoint = event.data.getLocalPosition(this.parent);
                    this.dragPoint.x = this.position.x;
                    this.dragPoint.y = this.position.y;
            }

            function onDragMove(event) {
                if (this.dragging) {
                    this.x += event.data.originalEvent.movementX;
                    this.y += event.data.originalEvent.movementY;
                }
            }

            function onDragEnd() {
                this.alpha = 1;
                this.dragging = false;
                material.SetPosition(this.x , this.y)
                // set the interaction data to null
                this.data = null;
            }
    }

    CreatePort(container,positionX,positionY, i){
        var NwCard = new pixi_NetWorkCard();
        NwCard.SetMaterial(this.material.network_cards, i);
        NwCard.getName(container);
        NwCard.SetPosition(container, positionX,positionY);
        NwCard.draw(container,this.renderer)
        var singleObj = {};
        singleObj['type'] = 'NetWorkCard' + i;
        singleObj['value'] = NwCard;
        this.NwCart.push(singleObj);
        

    }

     
    SetPosition(positionX: number , positionY:number){
        this.positionX = positionX;
        this.positionY = positionY;
    }
    remove(){

    }
}