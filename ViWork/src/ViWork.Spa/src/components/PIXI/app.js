PIXI.utils.sayHello();
let app = new PIXI.Application({ 
                width: 800,         // default: 800
                eight:600,        // default: 600
                antialias: true,    // default: false
                transparent: false, // default: false
                resolution: 1,       // default: 1       
               backgroundColor : 23921066	
});
app.renderer.view.style.position = "relative";
app.renderer.view.style.display = "block";
app.renderer.view.style.left ="50%";
app.renderer.view.style.top ="50%";
app.renderer.view.style.transform = "translate3d( -50%, -50%, 0 )";
document.getElementById('Canva').appendChild(app.renderer.view);
var stage = new PIXI.container();

 PIXI.loader.add("../../assets/BLAC.png").load(LoadImg);


function LoadImg(){

  logo = new PIXI.Sprites(
    PIXI.loader.ressources["../../assets/BLAC.png"].texture
  );
  stage.appendChild(logo);
         
};
render.render(stage)
