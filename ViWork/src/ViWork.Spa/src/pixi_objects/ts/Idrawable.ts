import * as PIXI from "pixi.js";

export interface Idrawable {
    material: any;
    texture: PIXI.Texture;
    sprite_path: string;
    container: PIXI.Container;
    sprite: PIXI.Sprite;

    take(positionX: number , positionY: number ): any;

    put(): any;

    draw(container: PIXI.Container, renderer:any): any;

    Move(material: any) : any;
    remove(): any;

}
