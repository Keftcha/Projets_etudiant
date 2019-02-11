import * as PIXI from 'pixi.js'


class PixiWrapper
{
    private app : any; 
    private canvasWidth : any;
    private canvasHeight : any;
    private _triggers : any;
    private _event : any;
    
    constructor(){
        this.app = null;
        this.canvasWidth = null;
        this.canvasHeight = null;
        this._triggers = {}; // event trigger
        this._event = {
            openModal: 'open-modal',
            wantWire: 'want-wire',
            globalMouseMove: 'global-mouse-move',
            globalMouseClick: 'global-mouse-click',
            containerMove: 'container-move',
            delete: 'delete',
            traceLog: 'trace-log'
        }
    }

    public initCanvas(canvas: any){
        this.purgeTriggers()
        this.canvasWidth = canvas.offsetWidth;
        this.canvasHeight = canvas.offsetHeight;

        this.app = new PIXI.Application(this.canvasWidth, this.canvasHeight, {
            view: canvas,
            backgroundColor: 0x424242
        });

        this.app.stage.interactive = true;
    }

    public purgeTriggers(){
        for(var trig in this._triggers){
            if(trig != this._event.openModal) { this._triggers[trig] = new Array(); }
        }
    }

    get PIXIApp(){
        if(this.app) { return this.app; }
    }

    get PIXI(){
        return PIXI;
    }

    get Width(){
        return this.canvasWidth;
    }

    get Height(){
        return this.canvasHeight;
    }

    public post(event: any, params: any){
        if( this._triggers[event] ) {
            for( var i in this._triggers[event] ){
                this._triggers[event][i](params);
            }
        }
    }

    public on(event: any, callback: any){
        if(!this._triggers[event]) { this._triggers[event] = new Array(); }
        this._triggers[event].push( callback );
    }

    public destroy(event: any, callback:any){
        for(var i in this._triggers[event]){
            if(this._triggers[event][i] == callback){
                this._triggers[event][i] = null;
                break;
            }
        }
        this._triggers[event] = this.filterByNull(this._triggers[event]);
    }

    public filterByNull(array: any){
        var data_return = Array();
        for(var i in array){
            if(array[i]) { data_return.push(array[i]); }
        }
        return data_return;
    }

    get Event(){
        return this._event;
    }


    public sleep(ms: any) {
        return new Promise((resolve) => setTimeout(resolve, ms));
    }

}

export default new PixiWrapper()
