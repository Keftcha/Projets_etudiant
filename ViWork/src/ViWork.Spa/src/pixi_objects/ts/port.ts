
import * as PIXI from "pixi.js"

import { Idrawable } from './Idrawable';
import { Port } from '@/objects/ts/port';
import { pixi_Cable } from './cable';

export class pixi_Port implements Idrawable{
    material: Port;
    cable: pixi_Cable;
    texture: PIXI.Texture;
    sprite_path: string;
    sprite: PIXI.Sprite;
    stage: PIXI.Container;
    link: boolean;
    container: PIXI.Container;
    positionX: any;
    positionY: any;
    id: any;
    type: any;


    constructor() {
       
        this.sprite_path =  process.env.VUE_APP_BACKEND+"/images/icons/ethernet_Off.png";
        this.type = "port"
    }

    take(positionX: number , positionY: number ) {
       
 
        // the computer change his sprite and his position is under the pointer while he don't put ip
    }

    put() {
        // put the computer at his new position (fixed position), don't forget to change his sprite
    }

    draw(container: PIXI.Container, renderer:any) {
   
        const sprite = PIXI.Sprite.fromImage(this.sprite_path)
    
        sprite.anchor.x = 0;
        sprite.anchor.y = 0;

        sprite.width = 20;
        sprite.height = 20;

             
        sprite.x = this.positionX + 100
        sprite.y = this.positionY + 100
        this.sprite= sprite;
        container.addChild(sprite);  
        this.stage = container;      
    }

    Move(sprite: PIXI.Sprite){

        
    }
     
    SetPosition(container: PIXI.Sprite,positionX: number , positionY:number){
        this.positionX = positionX;
        this.positionY = positionY;
    }

    ChangeTexture(){
        if (this.material.cable){
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
            if (NtC[i] === element) this.material = element
        });
        this.id = i;
    }
    remove(){

    }
    SetLink(value: boolean){
        this.link = value
    }

    onPlug(cable: pixi_Cable){
        
    }

}