//import {all} from "pixi.js"

import {NetworkCard} from "../../objects/ts/network_card"
import { Cable } from '../../objects/ts/cable';
import { Idrawable } from './Idrawable';
import { pixi_Cable } from './cable';
import * as PIXI from 'pixi.js';


export class pixi_NetWorkCard implements Idrawable {
    material: NetworkCard;
    texture: PIXI.Texture;
    sprite_path: string;
    container: PIXI.Container;
    sprite: PIXI.Sprite;
    stage: PIXI.Container;
    moosePositoin: any;
    positionX: any;
    positionY: any;
    link: boolean;
    cable: pixi_Cable;
    renderer: any;
    data: any;
    name: any;
    id: any;
    type: any;

    constructor() {
        this.sprite_path = process.env.VUE_APP_BACKEND+"/images/icons/ethernet_Off.png";
        this.type = "networkcard"       
    }

    take(positionX: number , positionY: number ) {
       
 
        // the computer change his sprite and his position is under the pointer while he don't put ip
    }

    put() {
        // put the computer at his new position (fixed position), don't forget to change his sprite
    }

    draw(container: PIXI.Container, renderer:any) {
        this.renderer = renderer;
        const sprite = PIXI.Sprite.fromImage(this.sprite_path)
        
    
        sprite.anchor.x = 0;
        sprite.anchor.y = 0;

        sprite.width = 20;
        sprite.height = 20;
                  
        sprite.x =this.positionX
        sprite.y = this.positionY
        this.sprite= sprite;
        this.Move(this);
        container.addChild(sprite);  
        this.stage = container;

         
    }

    Move(NtC: pixi_NetWorkCard){

    }
    getName(name : any){
        this.name = name;
    }

    ChangeTexture(){
        if (this.material.port.cable){
            var newSprite = process.env.VUE_APP_BACKEND+"/images/icons/ethernet_On.png";
            var texture = PIXI.Texture.fromImage(newSprite);
            this.sprite.texture = texture;
        } else {
            var sprite = process.env.VUE_APP_BACKEND+"/images/icons/ethernet_Off.png";
            var texture = PIXI.Texture.fromImage(sprite);
            this.sprite.texture = texture;
        }
    }

    SetMaterial(NtC , i){
        NtC.forEach(element => {
            if(NtC[i] === element) this.material = element
        });
        this.id = i;
    }
    

    SetPosition(container: PIXI.Sprite,positionX: number , positionY:number){
        this.positionX = positionX;
        this.positionY = positionY; 
    }
  
    remove(){

    }


    getCable(){
        return this.cable
    }



}