import * as PIXI from "pixi.js"

import {Idrawable} from"./Idrawable"
import {Cable} from "../../objects/ts/cable"
import { pixi_NetWorkCard } from './networkcard';


export class pixi_Cable implements Idrawable {
    material: Cable;
    texture: PIXI.Texture;
    sprite_path: string;
    container: PIXI.Container;
    sprite: PIXI.Sprite;
    receptor: any;
    destinator: any;
    color: any;
    cable: any;
    cable2: any;

    cross_eh: any;

    constructor(cross_eh: boolean) {
        this.material = new Cable(cross_eh)
        this.cross_eh = cross_eh;
        this.sprite_path = cross_eh ? process.env.VUE_APP_BACKEND+"/images/icons/cable_cross.png" : process.env.VUE_APP_BACKEND+"/images/icons/cable_straight.png"
       
    }
    
    take(positionX: number , positionY: number ) {
        
    
        // the computer change his sprite and his position is under the pointer while he don't put ip
    }
    GetColor(){
        if (this.cross_eh === true ){
            this.color = 0x9c5411
        } else {
            this.color = 0x979797
        }
    }

    put() {
        // put the computer at his new position (fixed position), don't forget to change his sprite
    }

    draw(container: PIXI.Container, renderer:any) {

        this.GetColor()
        this.container = container
        var cable = new PIXI.Graphics()
        var cable2 = new PIXI.Graphics()
        cable.beginFill(this.color);
        this.Drawing(cable,50,60,20);
            
        this.cable = cable;
        
        
       

        function animate(){      
            requestAnimationFrame(animate);
            renderer.render(container);           
        }
        animate();
                
        
    }
    Drawing(cable,n,p,i){
        this.GetColor();
        if(this.cable) this.cable.clear();
        if (this.receptor.type === "networkcard" && this.destinator.type === "networkcard"){
            cable
            .lineStyle(3,this.color)
            .moveTo(this.receptor.stage.position.x - n,this.receptor.stage.position.y - n + (this.receptor.id * i ))
            .lineTo(this.destinator.stage.position.x - n,this.destinator.stage.position.y - n + (this.destinator.id * i ))
            this.container.addChild(cable); 
        } else if (this.receptor.type === "networkcard" && this.destinator.type === "port"){
         
            cable
            .lineStyle(3,this.color)
            .moveTo(this.receptor.stage.position.x - n,this.receptor.stage.position.y - n + (this.receptor.id * i ))
            .lineTo(this.destinator.stage.position.x +(this.destinator.id * i )+p,this.destinator.sprite.position.y + 100)
            this.container.addChild(cable); 
        } else if (this.receptor.type === "port" && this.destinator.type === "networkcard"){

            cable
            .lineStyle(3,this.color)
            .moveTo(this.receptor.stage.position.x   +(this.receptor.id * i )+p,this.receptor.stage.position.y + 100 )
            .lineTo(this.destinator.stage.position.x - n,this.destinator.stage.position.y - n + (this.destinator.id * i ))
            this.container.addChild(cable); 
        } else if (this.receptor.type === "port" && this.destinator.type === "port"){
            cable
            .lineStyle(3,this.color)
            .moveTo(this.receptor.stage.position.x  +(this.receptor.id * i )+p ,this.receptor.stage.position.y +100)
            .lineTo(this.destinator.stage.position.x +(this.destinator.id * i )+p,this.destinator.stage.position.y + 100)
            this.container.addChild(cable); 
        }
    
       
    }

    SetReceptor(NtC: pixi_NetWorkCard){
        this.receptor = NtC;
    }

    SetDestinator(NtC: pixi_NetWorkCard){
        this.destinator = NtC
    }

    UpdateCable(NtC){
        this.GetColor();
        if (NtC.material !== this.destinator.material){
            this.Drawing(this.cable,50,60,20);
        } else if (NtC.material === this.destinator.material){
                this.cable.clear();
                if (this.receptor.type === "networkcard" && this.destinator.type === "networkcard"){
                    this.cable
                    .lineStyle(3,this.color)
                    .moveTo(this.receptor.stage.position.x - 50,this.receptor.stage.position.y - 50 + (this.receptor.id * 20 ))
                    .lineTo(this.destinator.stage.position.x - 50,this.destinator.stage.position.y - 50 + (this.destinator.id * 20 ))
                    this.container.addChild( this.cable); 
                } else if (this.receptor.type === "networkcard" && this.destinator.type === "port"){
                    this.cable
                    .lineStyle(3,this.color)
                    .moveTo(this.receptor.stage.position.x - 50,this.receptor.stage.position.y - 50 + (this.receptor.id * 20 ))
                    .lineTo(this.destinator.stage.position.x +(this.destinator.id * 20 )+60,this.destinator.stage.position.y + 100)
                    this.container.addChild( this.cable); 
                } else if (this.receptor.type === "port" && this.destinator.type === "networkcard"){
                    this.cable
                    .lineStyle(3,this.color)
                    .moveTo(this.receptor.stage.position.x   +(this.receptor.id * 20 )+60,this.receptor.stage.position.y + 100 )
                    .lineTo(this.destinator.stage.position.x - 50,this.destinator.stage.position.y - 50 + (this.destinator.id * 20 ))
                    this.container.addChild( this.cable); 
                } else if (this.receptor.type === "port" && this.destinator.type === "port"){
                    this.cable
                    .lineStyle(3,this.color)
                    .moveTo(this.receptor.stage.position.x  +(this.receptor.id * 20 )+60 ,this.receptor.stage.position.y +100)
                    .lineTo(this.destinator.stage.position.x +(this.destinator.id * 20 )+60,this.destinator.stage.position.y + 100)
                    this.container.addChild( this.cable); 
                }
            
            // } else  {
            //     this.cable.clear();
            //     this.cable
            //         .lineStyle(3,0x32CD32)
            //         .moveTo(this.receptor.stage.position.x,this.receptor.stage.position.y + 17)
            //         .lineTo(NtC.stage.position.x ,NtC.stage.position.y)
       
            //     this.cable2.clear()
            //     this.cable2
            //     .lineStyle(3,0x32CD32)
            //     .moveTo(this.receptor.stage.position.x,this.receptor.stage.position.y )
            //     .lineTo(this.destinator.stage.position.x ,this.destinator.stage.position.y +17)
           
            // }
        }
    }


    Move(sprite: PIXI.Sprite){

        sprite.interactive = true;
        sprite.buttonMode = true;
    }   

    async GetPosition(positionX: number , positionY:number){
        this.container.position.x = positionX;
        this.container.position.y = positionY;
    }
    remove() {
    }

}