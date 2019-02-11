import * as PIXI from'pixi.js'

import {Idrawable} from "./Idrawable";
import {Computer} from "../../objects/ts/computer";
import { pixi_NetWorkCard } from './networkcard';

export class pixi_Computer implements Idrawable {
    material: Computer;
    texture: PIXI.Texture;
    sprite_path: string;
    container: PIXI.Container;
    sprite: PIXI.Sprite;
    Mail: PIXI.Sprite;
    positionX: any;
    positionY: any;
    renderer: any;
    NwCart: any;
    previousMessage: any;

    constructor() {
        this.container = new PIXI.Container();
        this.NwCart = []
        this.sprite_path = process.env.VUE_APP_BACKEND+"/images/icons/computer.png";       
    }

    take(positionX: number , positionY: number ) {
       
        // the computer change his sprite and his position is under the pointer while he don't put ip
    }

    SetMaterial(nbCard, packetKeeping, Pin){
        this.material = new Computer(nbCard, packetKeeping, Pin);
    }

    put() {
        // put the computer at his new position (fixed position), don't forget to change his sprite
    }

    draw(container: PIXI.Container, renderer:any) {
        this.renderer = renderer;
        const sprite = PIXI.Sprite.fromImage(this.sprite_path)
        sprite.anchor.x = 0.5;
        sprite.anchor.y = 0.5;
       
        sprite.width = 110;
        sprite.height = 110;
        sprite.x = container.position.x;
        sprite.y = container.position.y;
        this.sprite = sprite;
        this.container.addChild(sprite)

        const mail = PIXI.Sprite.fromImage(process.env.VUE_APP_BACKEND+"/images/icons/envelope.png");
        mail.anchor.x = 0,5;
        mail.anchor.y = 0,5;

        mail.width = 20;
        mail.height = 20;

        mail.x = sprite.width/2 - 19
        mail.y = - (sprite.height /2);

        this.container.addChild(mail);
        this.Mail = mail;

        
        for (var i=0; i < this.material.network_cards.length; i++ ){
            this.CreatePort(this.container,-sprite.width/2,(20*i)-(sprite.height/2),i);
        }
        container.addChild(this.container);
        
        function animate(){      
            requestAnimationFrame(animate);
            renderer.render(container);
        }
        animate();
        
    }
    CheckLastRecv(){
        if (this.material.last_recv === this.previousMessage && this.previousMessage !== null)
        {

                var newSprite = process.env.VUE_APP_BACKEND+"/images/icons/message.png";
                var texture = PIXI.Texture.fromImage(newSprite);
                this.Mail.texture = texture;
             
        } else {
            var sprite = process.env.VUE_APP_BACKEND+"/images/icons/envelope.png";
            var texture = PIXI.Texture.fromImage(sprite);
            this.Mail.texture = texture;
        }
    }
    Move(material: pixi_Computer){

        this.container.interactive = true;
        this.container.buttonMode = true;
        this.container.on('mousedown', onDragStart)
		    .on('touchstart', onDragStart)
		    .on('mouseup', onDragEnd)
		    .on('mouseupoutside', onDragEnd)
		    .on('touchend', onDragEnd)
		    .on('touchendoutside', onDragEnd)
		    .on('mousemove', onDragMove)
            .on('touchmove', onDragMove)
       

            function onDragStart(event) {
                // store a reference to the data
                    this.data = event.data;
                    this.alpha = 0.5;
                    this.dragging = true;
            }

            function onDragMove(event) {
                if (this.dragging) {
                    //this.x += event.data.originalEvent.movementX;
                    //this.y += event.data.originalEvent.movementY;
                    this.dragPoint = event.data.getLocalPosition(this.parent);
                    this.x = this.dragPoint.x;
                    this.y = this.dragPoint.y;
                    material.NwCart.forEach(element => {
                        if(element.value.cable){
                            element.value.cable.UpdateCable(element.value)
                        }
                    });
                }
            }

            function onDragEnd() {
                this.alpha = 1;
                this.dragging = false;
                material.SetPosition(this.x ,this.y)
                // set the interaction data to null
                this.data = null;
            }
    }
     
    SetPosition(positionX: number , positionY:number){
        this.positionX = positionX;
        this.positionY = positionY;
        this.container.x = positionX;
        this.container.y = positionY;
    }

    remove(){

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

}