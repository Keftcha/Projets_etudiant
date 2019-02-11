<script>
export default {
  inject: ['EventBus', 'PIXIWrapper'],
  // x, y define the sprite's position in the parent.
  // imagePath is the path to the image on the server to render as the sprite.
  props: ['x', 'y', 'imagePath'],

  data() {
    return {
      sprite: null
    }
  },

  render(h) { return h() },

  created() {
   
    this.sprite = this.PIXIWrapper.PIXI.Sprite.fromImage(this.imagePath);
  
    // Set the initial position.
    this.sprite.x = this.x || 0;
    this.sprite.y = this.y || 0;
    this.sprite.anchor.set(0.5);

    // Opt-in to interactivity.
    this.sprite.interactive = true;

    // Forward the pointerdown event.
    this.sprite.on('pointerdown', () => this.$emit('pointerdown', this.sprite));
    // When the PIXI renderer starts.
    this.EventBus.$on('ready', () => {
      // Add the sprite to the parent container or the root app stage.
      if (this.$parent.container) {

        this.$parent.container.addChild(this.sprite);
         console.log('1');
      } else {
        console.log(this.PIXIWrapper.PIXIApp)
        this.PIXIWrapper.PIXIApp.stage.addChild(this.sprite);
         console.log('2');
      }

      // Emit an event for this sprite on every tick.
      // Great way to kill performance.
      //this.PIXIWrapper.PIXIApp.ticker.add(delta => this.$emit('tick', this.sprite, delta));
    })
  }
}
</script>