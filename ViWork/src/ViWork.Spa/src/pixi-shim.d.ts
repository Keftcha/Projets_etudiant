declare namespace PIXI {
  // from CONST
  export const VERSION: typeof CONST.VERSION;
  export const PI_2: typeof CONST.PI_2;
  export const RAD_TO_DEG: typeof CONST.RAD_TO_DEG;
  export const DEG_TO_RAD: typeof CONST.DEG_TO_RAD;
  export const RENDERER_TYPE: typeof CONST.RENDERER_TYPE;
  export const BLEND_MODES: typeof CONST.BLEND_MODES;
  export const DRAW_MODES: typeof CONST.DRAW_MODES;
  export const SCALE_MODES: typeof CONST.SCALE_MODES;
  export const WRAP_MODES: typeof CONST.WRAP_MODES;
  export const TRANSFORM_MODE: typeof CONST.TRANSFORM_MODE;
  export const PRECISION: typeof CONST.PRECISION;
  export const GC_MODES: typeof CONST.GC_MODES;
  export const SHAPES: typeof CONST.SHAPES;
  export const TEXT_GRADIENT: typeof CONST.TEXT_GRADIENT;
  export const UPDATE_PRIORITY: typeof CONST.UPDATE_PRIORITY;

  export function autoDetectRenderer(
      width: number,
      height: number,
      options?: PIXI.RendererOptions,
      forceCanvas?: boolean,
  ): PIXI.WebGLRenderer | PIXI.CanvasRenderer;
  export function autoDetectRenderer(options?: PIXI.RendererOptions): PIXI.WebGLRenderer | PIXI.CanvasRenderer;
  export const loader: PIXI.loaders.Loader;

  //////////////////////////////////////////////////////////////////////////////
  /////////////////////////////// SETTINGS///////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace settings {
      export let TARGET_FPMS: number;
      export let MIPMAP_TEXTURES: boolean;
      export let RESOLUTION: number;
      export let FILTER_RESOLUTION: number;
      export let SPRITE_MAX_TEXTURES: number;
      export let SPRITE_BATCH_SIZE: number;
      export let RETINA_PREFIX: RegExp;
      export const RENDER_OPTIONS: {
          view: HTMLCanvasElement | null;
          antialias: boolean;
          forceFXAA: boolean;
          autoResize: boolean;
          transparent: boolean;
          backgroundColor: number;
          clearBeforeRender: boolean;
          preserveDrawingBuffer: boolean;
          roundPixels: boolean;
          width: number;
          height: number;
          legacy: boolean;
      };
      export let TRANSFORM_MODE: number;
      export let GC_MODE: number;
      export let GC_MAX_IDLE: number;
      export let GC_MAX_CHECK_COUNT: number;
      export let WRAP_MODE: number;
      export let SCALE_MODE: number;
      export let PRECISION_VERTEX: string;
      export let PRECISION_FRAGMENT: string;
      export let PRECISION: string;
      export let UPLOADS_PER_FRAME: number;
      export let CAN_UPLOAD_SAME_BUFFER: boolean;
      export let MESH_CANVAS_PADDING: number;
  }

  //////////////////////////////////////////////////////////////////////////////
  ///////////////////////////// ACCESSIBILITY////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace accessibility {
      // accessibility
      export class AccessibilityManager {
          public debug: boolean;
          public renderer: SystemRenderer;

          protected div: HTMLElement;
          protected pool: HTMLElement[];
          protected renderId: number;
          protected children: AccessibleTarget[];
          protected isActive: boolean;
          constructor(renderer: CanvasRenderer | WebGLRenderer);

          public activate(): void;
          public deactivate(): void;

          public destroy(): void;

          protected updateAccessibleObjects(displayObject: DisplayObject): void;
          protected update(): void;
          protected capHitArea(hitArea: HitArea): void;
          protected addChild(displayObject: DisplayObject): void;
          protected _onClick(e: interaction.InteractionEvent): void;
          protected _onFocus(e: interaction.InteractionEvent): void;
          protected _onFocusOut(e: interaction.InteractionEvent): void;
          protected _onKeyDown(e: interaction.InteractionEvent): void;
          protected _onMouseMove(e: MouseEvent): void;
      }
      export interface AccessibleTarget {
          accessible: boolean;
          accessibleTitle: string | null;
          accessibleHint: string | null;
          tabIndex: number;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////////// CORE//////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  // const

  export namespace CONST {
      export const VERSION: string;
      export const PI_2: number;
      export const RAD_TO_DEG: number;
      export const DEG_TO_RAD: number;
      export const TARGET_FPMS: number;
      export const RENDERER_TYPE: {
          UNKNOWN: number;
          WEBGL: number;
          CANVAS: number;
      };
      export const BLEND_MODES: {
          NORMAL: number;
          ADD: number;
          MULTIPLY: number;
          SCREEN: number;
          OVERLAY: number;
          DARKEN: number;
          LIGHTEN: number;
          COLOR_DODGE: number;
          COLOR_BURN: number;
          HARD_LIGHT: number;
          SOFT_LIGHT: number;
          DIFFERENCE: number;
          EXCLUSION: number;
          HUE: number;
          SATURATION: number;
          COLOR: number;
          LUMINOSITY: number;
          NORMAL_NPM: number;
          ADD_NPM: number;
          SCREEN_NPM: number;
      };
      export const DRAW_MODES: {
          POINTS: number;
          LINES: number;
          LINE_LOOP: number;
          LINE_STRIP: number;
          TRIANGLES: number;
          TRIANGLE_STRIP: number;
          TRIANGLE_FAN: number;
      };
      export const SCALE_MODES: {
          LINEAR: number;
          NEAREST: number;
      };
      export const GC_MODES: {
          AUTO: number;
          MANUAL: number;
      };
      export const WRAP_MODES: {
          CLAMP: number;
          MIRRORED_REPEAT: number;
          REPEAT: number;
      };
      export const TRANSFORM_MODE: {
          DEFAULT: number;
          DYNAMIC: number;
          STATIC: number;
      };
      export const URL_FILE_EXTENSION: RegExp | string;
      export const DATA_URI: RegExp | string;
      export const SVG_SIZE: RegExp | string;
      export const SHAPES: {
          POLY: number;
          RECT: number;
          CIRC: number;
          ELIP: number;
          RREC: number;
      };
      export const PRECISION: {
          LOW: string;
          MEDIUM: string;
          HIGH: string;
      };
      export const TEXT_GRADIENT: {
          LINEAR_VERTICAL: number;
          LINEAR_HORIZONTAL: number;
      };
      export const UPDATE_PRIORITY: {
          INTERACTION: number;
          HIGH: number;
          NORMAL: number;
          LOW: number;
          UTILITY: number;
      };
  }

  // display

  export interface StageOptions {
      children?: boolean;
      texture?: boolean;
      baseTexture?: boolean;
  }

  export class Application {

      public renderer: PIXI.WebGLRenderer | PIXI.CanvasRenderer;
      public stage: Container;
      public ticker: ticker.Ticker;
      public loader: loaders.Loader;
      public readonly screen: Rectangle;
      public readonly view: HTMLCanvasElement;

      private _ticker: ticker.Ticker;
      constructor(options?: ApplicationOptions);
      constructor(
          width?: number,
          height?: number,
          options?: ApplicationOptions,
          noWebGL?: boolean,
          sharedTicker?: boolean,
          sharedLoader?: boolean,
      );

      public stop(): void;
      public start(): void;
      public render(): void;
      public destroy(removeView?: boolean, stageOptions?: StageOptions | boolean): void;
  }

  export interface DestroyOptions {
      children?: boolean;
      texture?: boolean;
      baseTexture?: boolean;
  }
  export class Bounds {
      public minX: number;
      public minY: number;
      public maxX: number;
      public maxY: number;
      public rect: Rectangle;

      public isEmpty(): boolean;
      public clear(): void;

      public getRectangle(rect?: Rectangle): Rectangle;
      public addPoint(point: Point): void;
      public addQuad(vertices: ArrayLike<number>): Bounds | undefined;
      public addFrame(transform: Transform, x0: number, y0: number, x1: number, y1: number): void;
      public addVertices(transform: Transform, vertices: ArrayLike<number>, beginOffset: number, endOffset: number): void;
      public addBounds(bounds: Bounds): void;
      public addBoundsMask(bounds: Bounds, mask: Bounds): void;
      public addBoundsArea(bounds: Bounds, area: Rectangle): void;
  }
  export class Container extends DisplayObject {
      // end extras.getChildByName

      public children: DisplayObject[];
      public width: number;
      public height: number;

      protected onChildrenChange: (...args: any[]) => void;
      // begin extras.getChildByName
      public getChildByName<T extends DisplayObject>(name: string): T;
      public addChild<T extends DisplayObject>(child: T, ...additionalChildren: DisplayObject[]): T;
      public addChildAt<T extends DisplayObject>(child: T, index: number): T;
      public swapChildren(child: DisplayObject, child2: DisplayObject): void;
      public getChildIndex(child: DisplayObject): number;
      public setChildIndex(child: DisplayObject, index: number): void;
      public getChildAt<T extends DisplayObject>(index: number): T;
      public removeChild<T extends DisplayObject>(child: DisplayObject): T;
      public removeChildAt<T extends DisplayObject>(index: number): T;
      public removeChildren<T extends DisplayObject>(beginIndex?: number, endIndex?: number): T[];
      public updateTransform(): void;
      public calculateBounds(): void;
      public renderWebGL(renderer: WebGLRenderer): void;
      public renderAdvancedWebGL(renderer: WebGLRenderer): void;
      public renderCanvas(renderer: CanvasRenderer): void;
      public destroy(options?: DestroyOptions | boolean): void;

      public once(event: "added" | "removed", fn: (displayObject: DisplayObject) => void, context?: any): this;
      // tslint:disable-next-line:ban-types forbidden-types
      public once(event: string, fn: Function, context?: any): this;
      public on(event: "added" | "removed", fn: (displayObject: DisplayObject) => void, context?: any): this;
      // tslint:disable-next-line:ban-types forbidden-types
      public on(event: string, fn: Function, context?: any): this;
      // tslint:disable-next-line:ban-types forbidden-types
      public off(event: "added" | "removed" | string, fn?: Function, context?: any): this;
      protected _calculateBounds(): void;
      protected containerUpdateTransform(): void;
      protected _renderWebGL(renderer: WebGLRenderer): void;
      protected _renderCanvas(renderer: CanvasRenderer): void;
  }

  export class DisplayObject extends utils.EventEmitter implements interaction.InteractiveTarget, accessibility.AccessibleTarget {
      public cacheAsBitmap: boolean;
      // end extras.cacheAsBitmap

      // begin extras.getChildByName
      public name: string | null;
      // end extras.getGlobalPosition

      // begin accessible target
      public accessible: boolean;
      public accessibleTitle: string | null;
      public accessibleHint: string | null;
      public tabIndex: number;
      // end accessible target

      // begin interactive target
      public interactive: boolean;
      public interactiveChildren: boolean;
      public hitArea: PIXI.Rectangle | PIXI.Circle | PIXI.Ellipse | PIXI.Polygon | PIXI.RoundedRectangle | PIXI.HitArea;
      public buttonMode: boolean;
      public cursor: string;
      public trackedPointers: { [key: number]: interaction.InteractionTrackingData };
      // depricated
      public defaultCursor: string;
      // end interactive target

      public transform: TransformBase;
      public alpha: number;
      public visible: boolean;
      public renderable: boolean;
      public parent: Container;
      public worldAlpha: number;
      public filterArea: Rectangle | null;
      public x: number;
      public y: number;
      public worldTransform: Matrix;
      public localTransform: Matrix;
      public position: Point | ObservablePoint;
      public scale: Point | ObservablePoint;
      public pivot: Point | ObservablePoint;
      public skew: ObservablePoint;
      public rotation: number;
      public worldVisible: boolean;
      public mask: PIXI.Graphics | PIXI.Sprite | null;
      public filters: Array<Filter<any>> | null;
      // begin extras.cacheAsBitmap
      protected _cacheAsBitmap: boolean;
      protected _cacheData: boolean;
      protected _filters: Array<Filter<any>> | null;
      protected _enabledFilters: Array<Filter<any>> | null;
      protected _bounds: Bounds;
      protected _boundsID: number;
      protected _lastBoundsID: number;
      protected _boundsRect: Rectangle;
      protected _localBoundsRect: Rectangle;
      protected _mask: PIXI.Graphics | PIXI.Sprite | null;
      protected readonly _destroyed: boolean;
      // end extras.getChildByName

      // begin extras.getGlobalPosition
      public getGlobalPosition(point?: Point, skipUpdate?: boolean): Point;

      public updateTransform(): void;
      public getBounds(skipUpdate?: boolean, rect?: Rectangle): Rectangle;
      public getLocalBounds(rect?: Rectangle): Rectangle;
      // creates and returns a new point
      public toGlobal(position: PointLike): Point;
      // modifies the x and y of the passed point and returns it
      public toGlobal<T extends PointLike>(position: PointLike, point?: T, skipUpdate?: boolean): T;
      // creates and returns a new point
      public toLocal(position: PointLike, from?: DisplayObject): Point;
      // modifies the x and y of the passed point and returns it
      public toLocal<T extends PointLike>(position: PointLike, from?: DisplayObject, point?: T, skipUpdate?: boolean): T;
      public renderWebGL(renderer: WebGLRenderer): void;
      public renderCanvas(renderer: CanvasRenderer): void;
      public setParent(container: Container): Container;
      public setTransform(
          x?: number,
          y?: number,
          scaleX?: number,
          scaleY?: number,
          rotation?: number,
          skewX?: number,
          skewY?: number,
          pivotX?: number,
          pivotY?: number,
      ): DisplayObject;
      public destroy(): void;

      public on(event: interaction.InteractionEventTypes, fn: (event: interaction.InteractionEvent) => void, context?: any): this;
      public once(event: interaction.InteractionEventTypes, fn: (event: interaction.InteractionEvent) => void, context?: any): this;
      public removeListener(event: interaction.InteractionEventTypes, fn?: (event: interaction.InteractionEvent) => void, context?: any): this;
      public removeAllListeners(event?: interaction.InteractionEventTypes): this;
      public off(event: interaction.InteractionEventTypes, fn?: (event: interaction.InteractionEvent) => void, context?: any): this;
      public addListener(event: interaction.InteractionEventTypes, fn: (event: interaction.InteractionEvent) => void, context?: any): this;
      protected _renderCachedWebGL(renderer: WebGLRenderer): void;
      protected _initCachedDisplayObject(renderer: WebGLRenderer): void;
      protected _renderCachedCanvas(renderer: CanvasRenderer): void;
      protected _initCachedDisplayObjectCanvas(renderer: CanvasRenderer): void;
      protected _calculateCachedBounds(): Rectangle;
      protected _getCachedLocalBounds(): Rectangle;
      protected _destroyCachedDisplayObject(): void;
      protected _cacheAsBitmapDestroy(options: boolean | any): void;
      protected displayObjectUpdateTransform(): void;
      protected _recursivePostUpdateTransform(): void;
  }

  export class TransformBase {
      public static IDENTITY: TransformBase;

      public worldTransform: Matrix;
      public localTransform: Matrix;
      protected _worldID: number;
      protected _parentID: number;
      public updateLocalTransform(): void;
      public updateTransform(parentTransform: TransformBase): void;
      public updateWorldTransform(parentTransform: TransformBase): void;
  }
  export class TransformStatic extends TransformBase {
      public position: ObservablePoint;
      public scale: ObservablePoint;
      public pivot: ObservablePoint;
      public skew: ObservablePoint;

      public rotation: number;

      protected _rotation: number;
      protected _sr?: number;
      protected _cr?: number;
      protected _cy?: number;
      protected _sy?: number;
      protected _sx?: number;
      protected _cx?: number;
      protected _localID: number;
      protected _currentLocalID: number;
      public updateSkew(): void;
      public setFromMatrix(matrix: Matrix): void;

      protected onChange(): void;
  }
  export class Transform extends TransformBase {

      public position: Point;
      public scale: Point;
      public skew: ObservablePoint;
      public pivot: Point;

      public rotation: number;

      protected _rotation: number;
      protected _sr?: number;
      protected _cr?: number;
      protected _cy?: number;
      protected _sy?: number;
      protected _sx?: number;
      protected _cx?: number;
      constructor();

      public updateSkew(): void;
      public setFromMatrix(matrix: Matrix): void;
  }

  // graphics

  export class GraphicsData {

      public lineWidth: number;
      public lineAlignment: number;
      public nativeLines: boolean;
      public lineColor: number;
      public lineAlpha: number;
      public fillColor: number;
      public fillAlpha: number;
      public fill: boolean;
      public shape: Circle | Rectangle | Ellipse | Polygon | RoundedRectangle | any;
      public type?: number;
      protected _lineTint: number;
      protected _fillTint: number;
      protected holes: Circle[] | Rectangle[] | Ellipse[] | Polygon[] | RoundedRectangle[] | any[];
      constructor(
          lineWidth: number,
          lineColor: number,
          lineAlpha: number,
          fillColor: number,
          fillAlpha: number,
          fill: boolean,
          nativeLines: boolean,
          shape: Circle | Rectangle | Ellipse | Polygon | RoundedRectangle | any,
          lineAlignment?: number,
      );
      public clone(): GraphicsData;
      public addHole(shape: Circle | Rectangle | Ellipse | Polygon | RoundedRectangle | any): void;
      public destroy(options?: DestroyOptions | boolean): void;
  }
  export class Graphics extends Container {
      public static CURVES: {
          adaptive: boolean;
          maxLength: number;
          minSegments: number;
          maxSegments: number;
      };

      public static _SPRITE_TEXTURE: Texture;

      public fillAlpha: number;
      public lineWidth: number;
      public nativeLines: boolean;
      public lineColor: number;
      public lineAlignment: number;
      public tint: number;
      public blendMode: number;
      public currentPath: GraphicsData;
      public isMask: boolean;
      public boundsPadding: number;
      public dirty: number;
      public canvasTintDirty: number;
      public fastRectDirty: number;
      public clearDirty: number;
      public boundsDirty: number;
      protected graphicsData: GraphicsData[];
      protected _prevTint: number;
      protected _webGL: any;
      protected _localBounds: Bounds;
      protected cachedSpriteDirty: boolean;
      protected _spriteRect: Rectangle;
      protected _fastRect: boolean;

      constructor(nativeLines?: boolean);

      public clone(): Graphics;
      public lineStyle(lineWidth?: number, color?: number, alpha?: number, alignment?: number): Graphics;
      public moveTo(x: number, y: number): Graphics;
      public lineTo(x: number, y: number): Graphics;
      public quadraticCurveTo(cpX: number, cpY: number, toX: number, toY: number): Graphics;
      public bezierCurveTo(cpX: number, cpY: number, cpX2: number, cpY2: number, toX: number, toY: number): Graphics;
      public arcTo(x1: number, y1: number, x2: number, y2: number, radius: number): Graphics;
      public arc(cx: number, cy: number, radius: number, startAngle: number, endAngle: number, anticlockwise?: boolean): Graphics;
      public beginFill(color: number, alpha?: number): Graphics;
      public endFill(): Graphics;
      public drawRect(x: number, y: number, width: number, height: number): Graphics;
      public drawRoundedRect(x: number, y: number, width: number, height: number, radius: number): Graphics;
      public drawCircle(x: number, y: number, radius: number): Graphics;
      public drawEllipse(x: number, y: number, width: number, height: number): Graphics;
      public drawPolygon(path: number[] | Point[] | Polygon): Graphics;
      public drawStar(x: number, y: number, points: number, radius: number, innerRadius: number, rotation?: number): Graphics;
      public clear(): Graphics;
      public isFastRect(): boolean;
      public containsPoint(point: Point): boolean;
      public updateLocalBounds(): void;
      public drawShape(shape: Circle | Rectangle | Ellipse | Polygon | RoundedRectangle | any): GraphicsData;
      public generateCanvasTexture(scaleMode?: number, resolution?: number): Texture;
      public closePath(): Graphics;
      public addHole(): Graphics;
      public destroy(options?: DestroyOptions | boolean): void;
      protected _quadraticCurveLength(fromX: number, fromY: number, cpX: number, cpY: number, toX: number, toY: number): number;
      protected _bezierCurveLength(
          fromX: number,
          fromY: number,
          cpX: number,
          cpY: number,
          cpX2: number,
          cpY2: number,
          toX: number,
          toY: number,
      ): number;
      protected _segmentsCount(length: number): number;
      protected _renderCanvas(renderer: CanvasRenderer): void;
      protected _calculateBounds(): Rectangle;
      protected _renderSpriteRect(renderer: PIXI.SystemRenderer): void;
  }
  export class CanvasGraphicsRenderer {
      constructor(renderer: SystemRenderer);
      public render(graphics: Graphics): void;
      public destroy(): void;
      protected updateGraphicsTint(graphics: Graphics): void;
      protected renderPolygon(points: Point[], close: boolean, context: CanvasRenderingContext2D): void;
  }
  export class GraphicsRenderer extends ObjectRenderer {
      public gl: WebGLRenderingContext;

      public CONTEXT_UID: number;

      protected graphicsDataPool: GraphicsData[];
      protected primitiveShader: PrimitiveShader;
      constructor(renderer: PIXI.CanvasRenderer);

      public destroy(): void;
      public render(graphics: Graphics): void;
      public getWebGLData(webGL: WebGLRenderingContext, type: number, nativeLines: number): WebGLGraphicsData;
      protected updateGraphics(graphics: PIXI.Graphics): void;
  }
  export class WebGLGraphicsData {

      public gl: WebGLRenderingContext;
      public color: number[];
      public points: Point[];
      public indices: number[];
      public buffer: WebGLBuffer;
      public indexBuffer: WebGLBuffer;
      public dirty: boolean;
      public glPoints: number[];
      public glIndices: number[];
      public shader: glCore.GLShader;
      public vao: glCore.VertexArrayObject;
      public nativeLines: boolean;
      constructor(gl: WebGLRenderingContext, shader: glCore.GLShader, attribsState: glCore.AttribState);

      public reset(): void;
      public upload(): void;
      public destroy(): void;
  }
  export class PrimitiveShader extends glCore.GLShader {}

  // math

  export namespace GroupD8 {
      export const E: number;
      export const SE: number;
      export const S: number;
      export const SW: number;
      export const W: number;
      export const NW: number;
      export const N: number;
      export const NE: number;
      export const MIRROR_HORIZONTAL: number;
      export const MIRROR_VERTICAL: number;

      export function uX(ind: number): number;
      export function uY(ind: number): number;
      export function vX(ind: number): number;
      export function vY(ind: number): number;
      export function inv(rotation: number): number;
      export function add(rotationSecond: number, rotationFirst: number): number;
      export function sub(rotationSecond: number, rotationFirst: number): number;
      export function rotate180(rotation: number): number;
      export function isVertical(rotation: number): boolean;
      export function byDirection(dx: number, dy: number): number;
      export function matrixAppendRotationInv(matrix: Matrix, rotation: number, tx: number, ty: number): void;
  }
  export class Matrix {

      public static IDENTITY: Matrix;
      public static TEMP_MATRIX: Matrix;

      public a: number;
      public b: number;
      public c: number;
      public d: number;
      public tx: number;
      public ty: number;
      constructor(a?: number, b?: number, c?: number, d?: number, tx?: number, ty?: number);

      public fromArray(array: number[]): void;
      public set(a: number, b: number, c: number, d: number, tx: number, ty: number): Matrix;
      public toArray(transpose?: boolean, out?: number[]): number[];
      public apply(pos: Point, newPos?: Point): Point;
      public applyInverse(pos: Point, newPos?: Point): Point;
      public translate(x: number, y: number): Matrix;
      public scale(x: number, y: number): Matrix;
      public rotate(angle: number): Matrix;
      public append(matrix: Matrix): Matrix;
      public setTransform(
          x: number,
          y: number,
          pivotX: number,
          pivotY: number,
          scaleX: number,
          scaleY: number,
          rotation: number,
          skewX: number,
          skewY: number,
      ): PIXI.Matrix;
      public prepend(matrix: Matrix): Matrix;
      public invert(): Matrix;
      public identity(): Matrix;
      public decompose(transform: TransformBase): TransformBase;
      public clone(): Matrix;
      public copy(matrix: Matrix): Matrix;
  }

  class PointLike {
      public x: number;
      public y: number;

      public set(x?: number, y?: number): void;
      public copy(point: PointLike): void;
  }

  export class ObservablePoint extends PointLike {
      public cb: () => any;
      public scope: any;
      constructor(cb: () => any, scope?: any, x?: number, y?: number);

      public clone(cb?: Function, scope?: any): ObservablePoint;
      public equals(p: Point | ObservablePoint | PointLike): boolean;
  }

  export class Point extends PointLike {
      constructor(x?: number, y?: number);

      public clone(): Point;
      public equals(p: PointLike): boolean;
  }

  export interface HitArea {
      contains(x: number, y: number): boolean;
  }
  export class Circle implements HitArea {

      public x: number;
      public y: number;
      public radius: number;
      public type: number;
      constructor(x?: number, y?: number, radius?: number);

      public clone(): Circle;
      public contains(x: number, y: number): boolean;
      public getBounds(): Rectangle;
  }
  export class Ellipse implements HitArea {

      public x: number;
      public y: number;
      public width: number;
      public height: number;
      public type: number;
      constructor(x?: number, y?: number, width?: number, height?: number);

      public clone(): Ellipse;
      public contains(x: number, y: number): boolean;
      public getBounds(): Rectangle;
  }
  export class Polygon implements HitArea {

      public closed: boolean;
      public points: number[];
      public type: number;
      constructor(points: Point[] | number[]);
      // Note - Rest Params cannot be combined with |
      // tslint:disable-next-line:unified-signatures
      constructor(...points: Point[]);
      // tslint:disable-next-line:unified-signatures
      constructor(...points: number[]);

      public clone(): Polygon;
      public contains(x: number, y: number): boolean;
      public close(): void;
  }
  export class Rectangle implements HitArea {

      public static EMPTY: Rectangle;

      public x: number;
      public y: number;
      public width: number;
      public height: number;
      public type: number;
      public left: number;
      public right: number;
      public top: number;
      public bottom: number;
      constructor(x?: number, y?: number, width?: number, height?: number);

      public clone(): Rectangle;
      public copy(rectangle: Rectangle): Rectangle;
      public contains(x: number, y: number): boolean;
      public pad(paddingX: number, paddingY: number): void;
      public fit(rectangle: Rectangle): void;
      public enlarge(rectangle: Rectangle): void;
  }
  export class RoundedRectangle implements HitArea {

      public x: number;
      public y: number;
      public width: number;
      public height: number;
      public radius: number;
      public type: number;
      constructor(x?: number, y?: number, width?: number, height?: number, radius?: number);

      public clone(): RoundedRectangle;
      public contains(x: number, y: number): boolean;
  }

  // renderers

  export interface RendererOptions {
      /**
       * the width of the renderers view [default=800]
       */
      width?: number;

      /**
       * the height of the renderers view [default=600]
       */
      height?: number;

      /**
       * the canvas to use as a view, optional
       */
      view?: HTMLCanvasElement;

      /**
       * If the render view is transparent, [default=false]
       */
      transparent?: boolean;

      /**
       * sets antialias (only applicable in chrome at the moment) [default=false]
       */
      antialias?: boolean;

      /**
       * enables drawing buffer preservation, enable this if you need to call toDataUrl on the webgl context [default=false]
       */
      preserveDrawingBuffer?: boolean;

      /**
       * The resolution / device pixel ratio of the renderer, retina would be 2 [default=1]
       */
      resolution?: number;

      /**
       * prevents selection of WebGL renderer, even if such is present [default=false]
       */
      forceCanvas?: boolean;

      /**
       * The background color of the rendered area (shown if not transparent) [default=0x000000]
       */
      backgroundColor?: number;

      /**
       * This sets if the renderer will clear the canvas or not before the new render pass. [default=true]
       */
      clearBeforeRender?: boolean;

      /**
       * If true Pixi will Math.floor() x/ y values when rendering, stopping pixel interpolation. [default=false]
       */
      roundPixels?: boolean;

      /**
       * forces FXAA antialiasing to be used over native FXAA is faster, but may not always look as great ** webgl only** [default=false]
       */
      forceFXAA?: boolean;

      /**
       * `true` to ensure compatibility with older / less advanced devices. If you experience unexplained flickering try setting this to true. **webgl only** [default=false]
       */
      legacy?: boolean;

      /**
       * Deprecated
       */
      context?: WebGLRenderingContext;

      /**
       * Deprecated
       */
      autoResize?: boolean;

      /**
       * Parameter passed to webgl context, set to "high-performance" for devices with dual graphics card
       */
      powerPreference?: string;
  }

  export interface ApplicationOptions extends RendererOptions {
      /**
       * `true` to use PIXI.ticker.shared, `false` to create new ticker. [default=false]
       */
      sharedTicker?: boolean;

      /**
       * `true` to use PIXI.loaders.shared, `false` to create new Loader.
       */
      sharedLoader?: boolean;

      /**
       * automatically starts the rendering after the construction.
       * Note that setting this parameter to false does NOT stop the shared ticker even if you set
       * options.sharedTicker to true in case that it is already started. Stop it by your own.
       */
      autoStart?: boolean;
  }

  interface DefaultRendererPlugins {
      accessibility: accessibility.AccessibilityManager;
      interaction: interaction.InteractionManager;
  }
  export interface RendererPlugins extends DefaultRendererPlugins {}
  export class SystemRenderer extends utils.EventEmitter {

      public type: number;
      public options: RendererOptions;
      public screen: Rectangle;
      public readonly width: number;
      public readonly height: number;
      public view: HTMLCanvasElement;
      public resolution: number;
      public transparent: boolean;
      public autoResize: boolean;
      public blendModes: any; // todo?
      public preserveDrawingBuffer: boolean;
      public clearBeforeRender: boolean;
      public roundPixels: boolean;
      public backgroundColor: number;
      protected _backgroundColor: number;
      protected _backgroundColorRgba: number[];
      protected _backgroundColorString: string;
      protected _tempDisplayObjectParent: Container;
      protected _lastObjectRendered: DisplayObject;
      constructor(system: string, options?: RendererOptions);
      constructor(system: string, screenWidth?: number, screenHeight?: number, options?: RendererOptions);

      public resize(screenWidth: number, screenHeight: number): void;
      public generateTexture(displayObject: DisplayObject, scaleMode?: number, resolution?: number, region?: Rectangle): RenderTexture;
      public render(...args: any[]): void;
      public destroy(removeView?: boolean): void;
  }
  interface DefaultCanvasRendererPlugins {
      extract: extract.CanvasExtract;
      prepare: prepare.CanvasPrepare;
  }
  export interface CanvasRendererPlugins extends DefaultCanvasRendererPlugins, RendererPlugins {}
  export class CanvasRenderer extends SystemRenderer {
      // plugintarget mixin start
      public static __plugins: { [pluginName: string]: { new (renderer: CanvasRenderer): any } };
      public static registerPlugin(pluginName: string, ctor: { new (renderer: CanvasRenderer): any }): void;
      public plugins: CanvasRendererPlugins;
      public rootContext: CanvasRenderingContext2D;
      public rootResolution?: number;
      public refresh: boolean;
      public maskManager: CanvasMaskManager;
      public smoothProperty: string;
      public extract: extract.CanvasExtract;

      public context: CanvasRenderingContext2D | null;

      protected _activeBlendMode: number;
      // plugintarget mixin end

      constructor(options?: RendererOptions);
      constructor(screenWidth?: number, screenHeight?: number, options?: RendererOptions);
      public initPlugins(): void;
      public destroyPlugins(): void;

      public render(
          displayObject: PIXI.DisplayObject,
          renderTexture?: PIXI.RenderTexture,
          clear?: boolean,
          transform?: PIXI.Matrix,
          skipUpdateTransform?: boolean,
      ): void;
      public setBlendMode(blendMode: number): void;
      public destroy(removeView?: boolean): void;
      public clear(clearColor?: string): void;
      public invalidateBlendMode(): void;

      public on(event: "prerender" | "postrender", fn: () => void, context?: any): this;
      public once(event: "prerender" | "postrender", fn: () => void, context?: any): this;
      public removeListener(event: "prerender" | "postrender", fn?: () => void, context?: any): this;
      public removeAllListeners(event?: "prerender" | "postrender"): this;
      public off(event: "prerender" | "postrender", fn?: () => void, context?: any): this;
      public addListener(event: "prerender" | "postrender", fn: () => void, context?: any): this;
  }
  export class CanvasMaskManager {
      constructor(renderer: CanvasRenderer);

      public pushMask(maskData: any): void;
      public popMask(renderer: WebGLRenderer | CanvasRenderer): void;
      public destroy(): void;
      protected renderGraphicsShape(graphics: Graphics): void;
  }
  export class CanvasRenderTarget {

      public canvas: HTMLCanvasElement;
      public context: CanvasRenderingContext2D;
      public resolution: number;

      public width: number;
      public height: number;
      constructor(width: number, height: number, resolution: number);

      public clear(): void;
      public resize(width: number, height: number): void;
      public destroy(): void;
  }

  export interface WebGLRendererOptions extends RendererOptions {}
  interface DefaultWebGLRendererPlugins {
      extract: extract.WebGLExtract;
      prepare: prepare.WebGLPrepare;
  }
  export interface WebGLRendererPlugins extends DefaultWebGLRendererPlugins, RendererPlugins {}
  export class WebGLRenderer extends SystemRenderer {
      // plugintarget mixin start
      public static __plugins: { [pluginName: string]: { new (renderer: WebGLRenderer): any } };
      public static registerPlugin(pluginName: string, ctor: { new (renderer: WebGLRenderer): any }): void;
      public plugins: WebGLRendererPlugins;
      public maskManager: MaskManager;
      public stencilManager?: StencilManager;
      public emptyRenderer: ObjectRenderer;
      public currentRenderer: ObjectRenderer;
      public gl: WebGLRenderingContext;
      public CONTEXT_UID: number;
      public state?: WebGLState;
      public renderingToScreen: boolean;
      public boundTextures: BaseTexture[];
      public filterManager: FilterManager;
      public textureManager?: TextureManager;
      public textureGC?: TextureGarbageCollector;
      public extract: extract.WebGLExtract;
      public _activeRenderTarget: RenderTarget;
      public handleContextLost: (event: WebGLContextEvent) => void;
      public handleContextRestored: () => void;

      protected _contextOptions: {
          alpha: boolean;
          antiAlias?: boolean;
          premultipliedAlpha: boolean;
          stencil: boolean;
          preseveDrawingBuffer?: boolean;
      };
      protected _backgroundColorRgba: number[];
      protected drawModes: any;
      protected _activeShader: Shader;
      // plugintarget mixin end

      constructor(options?: WebGLRendererOptions);
      constructor(screenWidth?: number, screenHeight?: number, options?: WebGLRendererOptions);
      public initPlugins(): void;
      public destroyPlugins(): void;

      public render(
          displayObject: PIXI.DisplayObject,
          renderTexture?: PIXI.RenderTexture,
          clear?: boolean,
          transform?: PIXI.Matrix,
          skipUpdateTransform?: boolean,
      ): void;
      public setObjectRenderer(objectRenderer: ObjectRenderer): void;
      public flush(): void;
      public setBlendMode(blendMode: number): void;
      public clear(clearColor?: number): void;
      public setTransform(matrix: Matrix): void;
      public clearRenderTexture(renderTexture: RenderTexture, clearColor?: number): WebGLRenderer;
      public bindRenderTexture(renderTexture: RenderTexture, transform: Matrix): WebGLRenderer;
      public bindRenderTarget(renderTarget: RenderTarget): WebGLRenderer;
      public bindShader(shader: Shader, autoProject?: boolean): WebGLRenderer;
      public bindTexture(texture: Texture | BaseTexture, location?: number, forceLocation?: boolean): number;
      public unbindTexture(texture: Texture | BaseTexture): WebGLRenderer | undefined;
      public createVao(): glCore.VertexArrayObject;
      public bindVao(vao: glCore.VertexArrayObject): WebGLRenderer;
      public reset(): WebGLRenderer;
      public destroy(removeView?: boolean): void;

      public on(event: "prerender" | "postrender", fn: () => void, context?: any): this;
      public on(event: "context", fn: (gl: WebGLRenderingContext) => void, context?: any): this;
      public once(event: "prerender" | "postrender", fn: () => void, context?: any): this;
      public once(event: "context", fn: (gl: WebGLRenderingContext) => void, context?: any): this;
      public removeListener(event: "prerender" | "postrender", fn?: () => void, context?: any): this;
      public removeListener(event: "context", fn?: (gl: WebGLRenderingContext) => void, context?: any): this;
      public removeAllListeners(event?: "prerender" | "postrender" | "context"): this;
      public off(event: "prerender" | "postrender", fn?: () => void, context?: any): this;
      public off(event: "context", fn?: (gl: WebGLRenderingContext) => void, context?: any): this;
      public addListener(event: "prerender" | "postrender", fn: () => void, context?: any): this;
      public addListener(event: "context", fn: (gl: WebGLRenderingContext) => void, context?: any): this;
      protected _initContext(): void;
  }
  export class WebGLState {

      public activeState: number[];
      public defaultState: number[];
      public stackIndex: number;
      public stack: number[];
      public gl: WebGLRenderingContext;
      public maxAttribs: number;
      public attribState: glCore.AttribState;
      public nativeVaoExtension: any;
      constructor(gl: WebGLRenderingContext);

      public push(): void;
      public pop(): void;
      public setState(state: number[]): void;
      public setBlend(value: number): void;
      public setBlendMode(value: number): void;
      public setDepthTest(value: number): void;
      public setCullFace(value: number): void;
      public setFrontFace(value: number): void;
      public resetAttributes(): void;
      public resetToDefault(): void;
  }
  export class TextureManager {

      public renderer: WebGLRenderer;
      public gl: WebGLRenderingContext;
      protected _managedTextures: Texture[];
      constructor(renderer: WebGLRenderer);

      public bindTexture(): void;
      public getTexture(): WebGLTexture;
      public updateTexture(texture: BaseTexture | Texture): WebGLTexture;
      public destroyTexture(texture: BaseTexture, _skipRemove?: boolean): void;
      public removeAll(): void;
      public destroy(): void;
  }
  export class TextureGarbageCollector {

      public renderer: WebGLRenderer;
      public count: number;
      public checkCount: number;
      public maxIdle: number;
      public checkCountMax: number;
      public mode: number;
      constructor(renderer: WebGLRenderer);

      public update(): void;
      public run(): void;
      public unload(displayObject: DisplayObject): void;
  }
  export abstract class ObjectRenderer extends WebGLManager {
      constructor(renderer: WebGLRenderer);

      public start(): void;
      public stop(): void;
      public flush(): void;

      public render(...args: any[]): void;
  }
  export class Quad {

      public gl: WebGLRenderingContext;
      public vertices: number[];
      public uvs: number[];
      public interleaved: number[];
      public indices: number[];
      public vertexBuffer: WebGLBuffer;
      public vao: glCore.VertexArrayObject;
      constructor(gl: WebGLRenderingContext);
      public initVao(shader: glCore.GLShader): void;
      public map(targetTextureFrame: Rectangle, destinationFrame: Rectangle): Quad;
      public upload(): Quad;
      public destroy(): void;
  }
  interface FilterDataStackItem {
      renderTarget: RenderTarget;
      filter: any[];
      bounds: Rectangle;
  }
  export class RenderTarget {

      public gl: WebGLRenderingContext;
      public frameBuffer: glCore.GLFramebuffer;
      public texture: Texture;
      public clearColor: number[];
      public size: Rectangle;
      public resolution: number;
      public projectionMatrix: Matrix;
      public transform: Matrix;
      public frame: Rectangle;
      public defaultFrame: Rectangle;
      public destinationFrame: Rectangle;
      public sourceFrame?: Rectangle;
      public stencilBuffer: glCore.GLFramebuffer;
      public stencilMaskStack: Graphics[];
      public filterData: {
          index: number;
          stack: FilterDataStackItem[];
      };
      public scaleMode: number;
      public root: boolean;
      protected filterPoolKey: string;

      constructor(gl: WebGLRenderingContext, width: number, height: number, scaleMode: number, resolution: number, root?: boolean);

      public clear(clearColor?: number[]): void;
      public attachStencilBuffer(): void;
      public setFrame(destinationFrame: Rectangle, sourceFrame: Rectangle): void;
      public activate(): void;
      public calculateProjection(destinationFrame: Rectangle, sourceFrame: Rectangle): void;
      public resize(width: number, height: number): void;
      public destroy(): void;
  }

  export class BlendModeManager extends WebGLManager {

      public currentBlendMode: number;
      constructor(renderer: WebGLRenderer);

      public setBlendMode(blendMode: number): boolean;
  }
  interface FilterManagerStackItem {
      renderTarget: RenderTarget;
      sourceFrame: Rectangle;
      destinationFrame: Rectangle;
      filters: Array<Filter<any>>;
      target: any;
      resolution: number;
  }
  export class FilterManager extends WebGLManager {

      public gl: WebGLRenderingContext;
      public quad: Quad;
      public stack: FilterManagerStackItem[];
      public stackIndex: number;
      public shaderCache: any;
      public filterData: any;

      protected _screenWidth: number;
      protected _screenHeight: number;
      constructor(renderer: WebGLRenderer);

      public onPrerender(): void;
      public pushFilter(target: RenderTarget, filters: Array<Filter<any>>): void;
      public popFilter(): void;
      public applyFilter(shader: glCore.GLShader | Filter<any>, inputTarget: RenderTarget, outputTarget: RenderTarget, clear?: boolean): void;
      public syncUniforms(shader: glCore.GLShader, filter: Filter<any>): void;
      public getRenderTarget(clear?: boolean, resolution?: number): RenderTarget;
      public returnRenderTarget(renderTarget: RenderTarget): RenderTarget;
      public calculateScreenSpaceMatrix(outputMatrix: Matrix): Matrix;
      public calculateNormalizedScreenSpaceMatrix(outputMatrix: Matrix): Matrix;
      public calculateSpriteMatrix(outputMatrix: Matrix, sprite: Sprite): Matrix;
      public destroy(contextLost?: boolean): void;
      public emptyPool(): void;
      public getPotRenderTarget(gl: WebGLRenderingContext, minWidth: number, minHeight: number, resolution: number): RenderTarget;
      public freePotRenderTarget(renderTarget: RenderTarget): void;
  }
  export class StencilMaskStack {
      public stencilStack: any[];
      public reverse: boolean;
      public count: number;
  }
  export class MaskManager extends WebGLManager {
      public scissor: boolean;
      public scissorData: any;
      public scissorRenderTarget: RenderTarget;
      public enableScissor: boolean;
      public alphaMaskPool: number[];
      public alphaMaskIndex: number;
      public pushMask(target: RenderTarget, maskData: Sprite | Graphics): void;
      public popMask(target: RenderTarget, maskData: Sprite | Graphics): void;
      public pushSpriteMask(target: RenderTarget, maskData: Sprite | Graphics): void;
      public popSpriteMask(): void;
      public pushStencilMask(maskData: Sprite | Graphics): void;
      public popStencilMask(): void;
      public pushScissorMask(target: RenderTarget, maskData: Sprite | Graphics): void;
      public popScissorMask(): void;
  }
  export class StencilManager extends WebGLManager {

      public stencilMaskStack: Graphics[];
      constructor(renderer: WebGLRenderer);

      public setMaskStack(stencilMasStack: Graphics[]): void;
      public pushStencil(graphics: Graphics): void;
      public popStencil(): void;
      public destroy(): void;

      protected _useCurrent(): void;
      protected _getBitwiseMask(): number;
  }
  export class WebGLManager {

      public renderer: WebGLRenderer;
      constructor(renderer: WebGLRenderer);
      public onContextChange(): void;
      public destroy(): void;
  }
  export interface UniformData<V> {
      type: string;
      value: V;

      // name is set by pixi if uniforms were automatically extracted from shader code, but not used anywhere
      name?: string;
  }
  type UniformDataMap<U> = { [K in keyof U]: UniformData<U[K]> };
  export class Filter<U extends object> {

      public static defaultVertexSrc: string;
      public static defaultFragmentSrc: string;
      public vertexSrc?: string;
      public fragmentSrc: string;
      public blendMode: number;
      public uniforms: U;
      public glShaders: any;
      public glShaderKey?: number;
      public padding: number;
      public resolution: number;
      public enabled: boolean;
      public autoFit: boolean;

      protected _blendMode: number;
      protected uniformData: UniformDataMap<U>;
      constructor(vertexSrc?: string, fragmentSrc?: string, uniformData?: UniformDataMap<U>);
      public apply(filterManager: FilterManager, input: RenderTarget, output: RenderTarget, clear?: boolean, currentState?: any): void;
  }
  interface SpriteMaskFilterUniforms {
      mask: Texture;
      otherMatrix: Matrix;
      alpha: number;
  }
  export class SpriteMaskFilter extends Filter<SpriteMaskFilterUniforms> {

      public maskSprite: Sprite;
      public maskMatrix: Matrix;
      constructor(sprite: Sprite);
      public apply(filterManager: FilterManager, input: RenderTarget, output: RenderTarget): void;
  }

  // sprites

  export class Sprite extends Container {

      public static from(source: number | string | BaseTexture | HTMLImageElement | HTMLCanvasElement | HTMLVideoElement): Sprite;
      public static fromFrame(frameId: string): Sprite;
      public static fromImage(imageId: string, crossorigin?: boolean, scaleMode?: number): Sprite;
      public anchor: ObservablePoint;
      public tint: number;
      public blendMode: number;
      public pluginName: string;
      public texture: Texture;
      public vertexData: Float32Array;
      public width: number;
      public height: number;

      protected _anchor: ObservablePoint;
      protected _texture: Texture;
      protected _transformTrimmedID: number;
      protected _textureTrimmedID: number;
      protected _width: number;
      protected _height: number;
      protected _tint: number;
      protected _tintRGB: number;
      protected cachedTint: number;
      protected textureDirty: boolean;
      protected _textureID: number;
      protected _transformID: number;
      protected vertexTrimmedData: Float32Array;
      constructor(texture?: Texture);
      public calculateVertices(): void;
      public getLocalBounds(): Rectangle;
      public containsPoint(point: Point): boolean;
      public destroy(options?: DestroyOptions | boolean): void;

      protected _onTextureUpdate(): void;
      protected _calculateBounds(): void;
      protected calculateTrimmedVertices(): void;
      protected onAnchorUpdate(): void;
      protected _renderWebGL(renderer: WebGLRenderer): void;
      protected _renderCanvas(renderer: CanvasRenderer): void;
  }
  export class BatchBuffer {
      public vertices: ArrayBuffer;
      public float32View: number[];
      public uint32View: number[];

      public destroy(): void;
  }
  export class SpriteRenderer extends ObjectRenderer {

      public vertSize: number;
      public vertByteSize: number;
      public size: number;
      public buffers: BatchBuffer[];
      public indices: number[];
      public shaders: Shader[];
      public currentIndex: number;
      public tick: number;
      public groups: any[];
      public sprites: Sprite[];
      public vertexBuffers: number[];
      public vaos: glCore.VertexArrayObject[];
      public vaoMax: number;
      public vertexCount: number;

      protected onContextChanged: () => void;
      protected onPrerender: () => void;
      constructor(renderer: PIXI.WebGLRenderer);
      public render(sprite: Sprite): void;
      public flush(): void;
      public start(): void;
      public stop(): void;
      public destroy(): void;
  }
  export class CanvasSpriteRenderer extends ObjectRenderer {
      constructor(renderer: WebGLRenderer);

      public render(sprite: Sprite): void;
      public destroy(): void;
  }
  export namespace CanvasTinter {
      export function getTintedTexture(sprite: Sprite, color: number): HTMLCanvasElement;
      export function tintWithMultiply(texture: Texture, color: number, canvas: HTMLCanvasElement): void;
      export function tintWithOverlay(texture: Texture, color: number, canvas: HTMLCanvasElement): void;
      export function tintWithPerPixel(texture: Texture, color: number, canvas: HTMLCanvasElement): void;
      export function roundColor(color: number): number;

      export let cacheStepsPerColorChannel: number;
      export let convertTintToImage: boolean;
      export let canUseMultiply: boolean;
      export let tintMethod: number;
  }

  // text
  export interface TextStyleOptions {
      align?: string;
      breakWords?: boolean;
      dropShadow?: boolean;
      dropShadowAlpha?: number;
      dropShadowAngle?: number;
      dropShadowBlur?: number;
      dropShadowColor?: string | number;
      dropShadowDistance?: number;
      fill?: string | string[] | number | number[] | CanvasGradient | CanvasPattern;
      fillGradientType?: number;
      fillGradientStops?: number[];
      fontFamily?: string | string[];
      fontSize?: number | string;
      fontStyle?: string;
      fontVariant?: string;
      fontWeight?: string;
      letterSpacing?: number;
      lineHeight?: number;
      lineJoin?: string;
      miterLimit?: number;
      padding?: number;
      stroke?: string | number;
      strokeThickness?: number;
      textBaseline?: string;
      trim?: boolean;
      whiteSpace?: string;
      wordWrap?: boolean;
      wordWrapWidth?: number;
      leading?: number;
  }

  export class TextStyle implements TextStyleOptions {

      public styleID: number;
      public align: string;
      public breakWords: boolean;
      public dropShadow: boolean;
      public dropShadowAlpha: number;
      public dropShadowAngle: number;
      public dropShadowBlur: number;
      public dropShadowColor: string | number;
      public dropShadowDistance: number;
      public fill: string | string[] | number | number[] | CanvasGradient | CanvasPattern;
      public fillGradientType: number;
      public fillGradientStops: number[];
      public fontFamily: string | string[];
      public fontSize: number | string;
      public fontStyle: string;
      public fontVariant: string;
      public fontWeight: string;
      public leading: number;
      public letterSpacing: number;
      public lineHeight: number;
      public lineJoin: string;
      public miterLimit: number;
      public padding: number;
      public stroke: string | number;
      public strokeThickness: number;
      public textBaseline: string;
      public trim: boolean;
      public whiteSpace: string;
      public wordWrap: boolean;
      public wordWrapWidth: number;

      protected _align: string;
      protected _breakWords: boolean;
      protected _dropShadow: boolean;
      protected _dropShadowAlpha: number;
      protected _dropShadowAngle: number;
      protected _dropShadowBlur: number;
      protected _dropShadowColor: string | number;
      protected _dropShadowDistance: number;
      protected _fill: string | string[] | number | number[] | CanvasGradient | CanvasPattern;
      protected _fillGradientType: number;
      protected _fillGradientStops: number[];
      protected _fontFamily: string | string[];
      protected _fontSize: number | string;
      protected _fontStyle: string;
      protected _fontVariant: string;
      protected _fontWeight: string;
      protected _leading: number;
      protected _letterSpacing: number;
      protected _lineHeight: number;
      protected _lineJoin: string;
      protected _miterLimit: number;
      protected _padding: number;
      protected _stroke: string | number;
      protected _strokeThickness: number;
      protected _textBaseline: string;
      protected _trim: boolean;
      protected _whiteSpace: string;
      protected _wordWrap: boolean;
      protected _wordWrapWidth: number;
      constructor(style: TextStyleOptions);

      public clone(): TextStyle;
      public reset(): void;

      public toFontString(): string;
  }

  export class TextMetrics {
      public static METRICS_STRING: string;
      public static BASELINE_SYMBOL: string;
      public static BASELINE_MULTIPLIER: number;

      public static _canvas: HTMLCanvasElement;
      public static _context: CanvasRenderingContext2D;
      public static _fonts: FontMetrics;
      public static _newLines: number[];
      public static _breakingSpaces: number[];

      public static measureText(text: string, style: TextStyle, wordWrap?: boolean, canvas?: HTMLCanvasElement): TextMetrics;
      public static wordWrap(text: string, style: TextStyle, canvas?: HTMLCanvasElement): string;
      public static addLine(line: string, newLine?: boolean): string;
      public static getFromCache(key: string, letterSpacing: number, cache: any, context: CanvasRenderingContext2D): number;
      public static collapseSpaces(whiteSpace?: string): boolean;
      public static collapseNewlines(whiteSpace?: string): boolean;
      public static trimRight(text?: string): string;
      public static isNewline(char?: string): boolean;
      public static isBreakingSpace(char?: string): boolean;
      public static tokenize(text?: string): string[];
      public static canBreakWords(token?: string, breakWords?: boolean): boolean;
      public static canBreakChars(char: string, nextChar: string, token: string, index: number, breakWords?: boolean): boolean;
      public static measureFont(font: string): FontMetrics;
      public static clearMetrics(font: string): void;

      public text: string;
      public style: TextStyle;
      public width: number;
      public height: number;
      public lines: number[];
      public lineWidths: number[];
      public lineHeight: number;
      public maxLineWidth: number;
      public fontProperties: any;

      constructor(
          text: string,
          style: TextStyle,
          width: number,
          height: number,
          lines: number[],
          lineWidths: number[],
          lineHeight: number,
          maxLineWidth: number,
          fontProperties: any,
      );
  }

  interface FontMetrics {
      ascent: number;
      descent: number;
      fontSize: number;
  }

  export class Text extends Sprite {

      public canvas: HTMLCanvasElement;
      public context: CanvasRenderingContext2D;
      public resolution: number;

      public width: number;
      public height: number;
      public style: TextStyle;
      public text: string;
      public dirty: boolean;
      protected _text: string;
      protected _style: TextStyle;
      // tslint:disable-next-line:ban-types forbidden-types
      protected _styleListener: Function;
      protected _font: string;
      protected localStyleID: number;
      protected _onStyleChange: () => void;
      constructor(text?: string, style?: TextStyleOptions, canvas?: HTMLCanvasElement);
      public renderWebGL(renderer: WebGLRenderer): void;
      public getLocalBounds(rect?: Rectangle): Rectangle;
      public destroy(options?: DestroyOptions | boolean): void;

      protected updateText(respectDirty?: boolean): void;
      protected drawLetterSpacing(text: string, x: number, y: number, isStroke?: boolean): void;
      protected updateTexture(): void;
      protected _renderCanvas(renderer: CanvasRenderer): void;
      protected _calculateBounds(): void;
      protected _generateFillStyle(style: TextStyle, lines: string[]): string | number | CanvasGradient;
  }

  // textures

  export class BaseRenderTexture extends BaseTexture {

      public height: number;
      public width: number;
      public realHeight: number;
      public realWidth: number;
      public resolution: number;
      public scaleMode: number;
      public hasLoaded: boolean;
      public valid: boolean;
      protected _glRenderTargets: { [n: number]: WebGLTexture };
      protected _canvasRenderTarget: { [n: number]: WebGLTexture };
      constructor(width?: number, height?: number, scaleMode?: number, resolution?: number);

      public resize(width: number, height: number): void;
      public destroy(): void;

      public on(event: "update", fn: (baseRenderTexture: BaseRenderTexture) => void, context?: any): this;
      public once(event: "update", fn: (baseRenderTexture: BaseRenderTexture) => void, context?: any): this;
      public removeListener(event: "update", fn?: (baseRenderTexture: BaseRenderTexture) => void, context?: any): this;
      public removeAllListeners(event?: "update"): this;
      public off(event: "update", fn?: (baseRenderTexture: BaseRenderTexture) => void, context?: any): this;
      public addListener(event: "update", fn: (baseRenderTexture: BaseRenderTexture) => void, context?: any): this;
  }
  export class BaseTexture extends utils.EventEmitter {
      public static from(source: string | HTMLImageElement | HTMLCanvasElement, scaleMode?: number, sourceScale?: number): BaseTexture;

      public static fromImage(imageUrl: string, crossorigin?: boolean, scaleMode?: number, sourceScale?: number): BaseTexture;
      public static fromCanvas(canvas: HTMLCanvasElement, scaleMode?: number, origin?: string): BaseTexture;
      public static addToCache(baseTexture: BaseTexture, id: string): void;
      public static removeFromCache(baseTexture: string | BaseTexture): BaseTexture;
      public resolution: number;
      public width: number;
      public height: number;
      public realWidth: number;
      public realHeight: number;
      public scaleMode: number;
      public hasLoaded: boolean;
      public isLoading: boolean;
      public wrapMode: number;
      public source: HTMLImageElement | HTMLCanvasElement | HTMLVideoElement | null;
      public origSource: HTMLImageElement | null;
      public imageType: string | null;
      public sourceScale: number;
      public premultipliedAlpha: boolean;
      public imageUrl: string | null;
      public mipmap: boolean;
      public wrap?: boolean;
      public textureCacheIds: string[];

      protected uuid?: number;
      protected touched: number;
      protected isPowerOfTwo: boolean;
      protected _glTextures: any;
      protected _enabled: number;
      protected _id?: number;
      protected _virtualBoundId: number;
      protected readonly _destroyed: boolean;

      constructor(source?: HTMLImageElement | HTMLCanvasElement | HTMLVideoElement, scaleMode?: number, resolution?: number);

      public update(): void;
      public destroy(): void;
      public dispose(): void;
      public updateSourceImage(newSrc: string): void;

      public on(event: "update" | "loaded" | "error" | "dispose", fn: (baseTexture: BaseTexture) => void, context?: any): this;
      public once(event: "update" | "loaded" | "error" | "dispose", fn: (baseTexture: BaseTexture) => void, context?: any): this;
      public removeListener(event: "update" | "loaded" | "error" | "dispose", fn?: (baseTexture: BaseTexture) => void, context?: any): this;
      public removeAllListeners(event?: "update" | "loaded" | "error" | "dispose"): this;
      public off(event: "update" | "loaded" | "error" | "dispose", fn?: (baseTexture: BaseTexture) => void, context?: any): this;
      public addListener(event: "update" | "loaded" | "error" | "dispose", fn: (baseTexture: BaseTexture) => void, context?: any): this;
      protected _updateDimensions(): void;
      protected _updateImageType(): void;
      protected _loadSvgSource(): void;
      protected _loadSvgSourceUsingDataUri(dataUri: string): void;
      protected _loadSvgSourceUsingXhr(): void;
      protected _loadSvgSourceUsingString(svgString: string): void;
      protected loadSource(source: HTMLImageElement | HTMLCanvasElement | HTMLVideoElement): void;
      protected _sourceLoaded(): void;
  }
  export class RenderTexture extends Texture {

      public static create(width?: number, height?: number, scaleMode?: number, resolution?: number): RenderTexture;
      public valid: boolean;

      protected legacyRenderer: any;
      constructor(baseRenderTexture: BaseRenderTexture, frame?: Rectangle);

      public resize(width: number, height: number, doNotResizeBaseTexture?: boolean): void;
  }
  export class Texture extends utils.EventEmitter {

      public static EMPTY: Texture;
      public static WHITE: Texture;

      public static fromImage(imageUrl: string, crossOrigin?: boolean, scaleMode?: number, sourceScale?: number): Texture;
      public static fromFrame(frameId: string): Texture;
      public static fromCanvas(canvas: HTMLCanvasElement, scaleMode?: number, origin?: string): Texture;
      public static fromVideo(video: HTMLVideoElement | string, scaleMode?: number, crossorigin?: boolean, autoPlay?: boolean): Texture;
      public static fromVideoUrl(videoUrl: string, scaleMode?: number, crossorigin?: boolean, autoPlay?: boolean): Texture;
      public static from(source: number | string | HTMLImageElement | HTMLCanvasElement | HTMLVideoElement | BaseTexture): Texture;
      public static fromLoader(source: HTMLImageElement | HTMLCanvasElement, imageUrl: string, name?: string): Texture;
      public static addToCache(texture: Texture, id: string): void;
      public static removeFromCache(texture: string | Texture): Texture;

      // depreciation
      public static addTextureToCache(texture: Texture, id: string): void;
      public static removeTextureFromCache(id: string): Texture;

      public noFrame: boolean;
      public baseTexture: BaseTexture;
      public trim?: Rectangle;
      public valid: boolean;
      public requiresUpdate: boolean;
      public orig: Rectangle;
      public defaultAnchor: Point;
      public transform: TextureMatrix;
      public textureCacheIds: string[];

      public frame: Rectangle;
      public rotate: number;
      public width: number;
      public height: number;
      protected _frame: Rectangle;
      protected _uvs: TextureUvs;
      protected _updateID: number;
      protected _rotate: boolean | 0;
      constructor(baseTexture: BaseTexture, frame?: Rectangle, orig?: Rectangle, trim?: Rectangle, rotate?: number, anchor?: Point);

      public update(): void;
      public destroy(destroyBase?: boolean): void;
      public clone(): Texture;
      public _updateUvs(): void;

      public on(event: "update", fn: (texture: Texture) => void, context?: any): this;
      public once(event: "update", fn: (texture: Texture) => void, context?: any): this;
      public removeListener(event: "update", fn?: (texture: Texture) => void, context?: any): this;
      public removeAllListeners(event?: "update"): this;
      public off(event: "update", fn?: (texture: Texture) => void, context?: any): this;
      public addListener(event: "update", fn: (texture: Texture) => void, context?: any): this;
      protected onBaseTextureLoaded(baseTexture: BaseTexture): void;
      protected onBaseTextureUpdated(baseTexture: BaseTexture): void;
  }
  export class TextureMatrix {
      public mapCoord: Matrix;
      public uClampFrame: Float32Array;
      public uClampOffset: Float32Array;

      public clampOffset: number;
      public clampMargin: number;

      public texture: Texture;

      protected _texture: Texture;
      protected _lastTextureID: number;
      constructor(texture: Texture, clampMargin?: number);

      public update(forceUpdate?: boolean): boolean;
      public multiplyUvs(uvs: Float32Array, out?: Float32Array): Float32Array;
  }
  export class TextureUvs {
      public x0: number;
      public y0: number;
      public x1: number;
      public y1: number;
      public x2: number;
      public y2: number;
      public x3: number;
      public y3: number;

      public uvsUint32: Uint32Array;

      protected set(frame: Rectangle, baseFrame: Rectangle, rotate: number): void;
  }

  export class Spritesheet {
      public static BATCH_SIZE: number;

      public baseTexture: BaseTexture;
      public animations: { [key: string]: Texture[]};
      public textures: { [key: string]: Texture };
      public data: any;
      public resolution: number;
      protected _frames: any;
      protected _frameKeys: string;
      protected _batchIndex: number;
      protected _callback: (spriteSheet: this, textures: { [key: string]: Texture }) => void;

      constructor(baseTexture: BaseTexture, data: any, resolutionFilename?: string);
      public parse(callback: (spriteSheet: this, textures: { [key: string]: Texture }) => void): void;
      public destroy(destroyBase?: boolean): void;
      protected _updateResolution(resolutionFilename: string): number;
      protected _processFrames(initialFrameIndex: number): void;
      protected _parseComplete(): void;
      protected _processAnimations(): void;
      protected _nextBatch(): void;
  }

  export class VideoBaseTexture extends BaseTexture {

      public static fromVideo(video: HTMLVideoElement, scaleMode?: number, autoPlay?: boolean): VideoBaseTexture;
      public static fromUrl(videoSrc: string | any | string[] | any[], crossorigin?: boolean, autoPlay?: boolean): VideoBaseTexture;
      public static fromUrls(videoSrc: string | any | string[] | any[]): VideoBaseTexture;

      public autoUpdate: boolean;
      public autoPlay: boolean;

      public source: HTMLVideoElement;
      protected _isAutoUpdating: boolean;
      constructor(source: HTMLVideoElement, scaleMode?: number, autoPlay?: boolean);

      public update(): void;
      public destroy(): void;
      protected _onCanPlay(): void;
      protected _onPlayStart(): void;
      protected _onPlayStop(): void;
      protected _isSourcePlaying(): boolean;
      protected _isSourceReady(): boolean;
      protected loadSource(source: HTMLVideoElement): void;
  }

  // ticker

  namespace ticker {
      export const shared: Ticker;

      export class TickerListener {

          public fn: (deltaTime: number) => void;
          public context: any;
          public priority: number;
          public once: boolean;
          public next: TickerListener;
          public previous: TickerListener;

          protected _destroyed: boolean;
          constructor(fn: (deltaTime: number) => void, context?: any, priority?: number, once?: boolean);
          public match(fn: (deltaTime: number) => void, context?: any): boolean;
          public emit(deltaTime: number): TickerListener;
          public connect(previous: TickerListener): void;
          public destroy(hard?: boolean): void;
      }
      export class Ticker {

          public autoStart: boolean;
          public deltaTime: number;
          public elapsedMS: number;
          public lastTime: number;
          public speed: number;
          public started: boolean;

          public readonly FPS: number;
          public minFPS: number;
          protected _tick: (time: number) => void;
          protected _head: TickerListener;
          protected _requestId: number | null;
          protected _maxElapsedMS: number;

          public add(fn: (deltaTime: number) => void, context?: any, priority?: number): Ticker;
          public addOnce(fn: (deltaTime: number) => void, context?: any, priority?: number): Ticker;
          // tslint:disable-next-line:ban-types forbidden-types
          public remove(fn: Function, context?: any, priority?: number): Ticker;

          public start(): void;
          public stop(): void;
          public destroy(): void;
          public update(currentTime?: number): void;

          protected _requestIfNeeded(): void;
          protected _cancelIfNeeded(): void;
          protected _startIfPossible(): void;

          protected _addListener(listener: TickerListener): Ticker;
      }
  }

  // shader

  export class Shader extends glCore.GLShader {
      constructor(
          gl: WebGLRenderingContext,
          vertexSrc: string | string[],
          fragmentSrc: string | string[],
          attributeLocations?: { [key: string]: number },
          precision?: string,
      );
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////// EXTRACT///////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace extract {
      export class CanvasExtract {
          protected renderer: CanvasRenderer;

          constructor(renderer: CanvasRenderer);

          public image(target?: DisplayObject | RenderTexture): HTMLImageElement;
          public base64(target?: DisplayObject | RenderTexture): string;
          public canvas(target?: DisplayObject | RenderTexture): HTMLCanvasElement;
          public pixels(renderTexture?: DisplayObject | RenderTexture): Uint8ClampedArray;

          public destroy(): void;
      }
      export class WebGLExtract {
          protected renderer: WebGLRenderer;

          constructor(renderer: WebGLRenderer);

          public image(target?: DisplayObject | RenderTexture): HTMLImageElement;
          public base64(target?: DisplayObject | RenderTexture): string;
          public canvas(target?: DisplayObject | RenderTexture): HTMLCanvasElement;
          public pixels(renderTexture?: DisplayObject | RenderTexture): Uint8Array;

          public destroy(): void;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////// EXTRAS////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace extras {
      export interface BitmapTextStyle {
          font?:
              | string
              | {
                    name?: string;
                    size?: number;
                };
          align?: string;
          tint?: number;
      }
      export class BitmapText extends Container {

          public static fonts: any;
          public static registerFont(xml: XMLDocument, textures: Texture | Texture[] | { [key: string]: Texture }): any;

          public letterSpacing: number;
          public textWidth: number;
          public textHeight: number;
          public font:
              | string
              | {
                    name?: string;
                    size?: number;
                };
          public maxWidth: number;
          public maxLineHeight: number;
          public dirty: boolean;
          public tint: number;
          public align: string;
          public text: string;
          public anchor: PIXI.Point | number;
          protected _letterSpacing: number;
          protected _textWidth: number;
          protected _textHeight: number;
          protected _glyphs: Sprite[];
          protected _font:
              | string
              | {
                    name?: string;
                    size?: number;
                };
          protected _text: string;
          protected _maxWidth: number;
          protected _maxLineHeight: number;
          protected _anchor: ObservablePoint;

          constructor(text: string, style?: BitmapTextStyle);
          public updateTransform(): void;
          public getLocalBounds(): Rectangle;

          protected updateText(): void;
          protected validate(): void;
      }
      interface AnimatedSpriteTextureTimeObject {
          texture: Texture;
          time?: number;
      }
      export class AnimatedSprite extends Sprite {

          public static fromFrames(frame: string[]): AnimatedSprite;
          public static fromImages(images: string[]): AnimatedSprite;
          public textures: Texture[] | AnimatedSpriteTextureTimeObject[];
          public animationSpeed: number;
          public loop: boolean;
          public onComplete: () => void;
          public onFrameChange: (currentFrame: number) => void;
          public onLoop: () => void;
          public playing: boolean;
          public totalFrames: number;
          public currentFrame: number;

          protected _autoUpdate: boolean;
          protected _textures: Texture[];
          protected _durations: number[];
          protected _currentTime: number;
          constructor(textures: Texture[] | AnimatedSpriteTextureTimeObject[], autoUpdate?: boolean);
          public stop(): void;
          public play(): void;
          public gotoAndStop(frameNumber: number): void;
          public gotoAndPlay(frameNumber: number): void;
          public destroy(options?: DestroyOptions | boolean): void;
          protected update(deltaTime: number): void;
      }
      export class TilingSprite extends Sprite {

          public static from(
              source: number | string | BaseTexture | HTMLCanvasElement | HTMLVideoElement,
              width?: number,
              height?: number,
          ): TilingSprite;
          public static fromFrame(frameId: string, width?: number, height?: number): TilingSprite;
          // if you remove the next line, the class will break. https://github.com/pixijs/pixi-typescript/issues/96
          public static fromImage(imageId: string, crossorigin?: boolean, scaleMode?: number): Sprite;
          public static fromImage(imageId: string, width?: number, height?: number, crossorigin?: boolean, scaleMode?: number): TilingSprite;

          public tileTransform: TransformStatic;
          public uvTransform: TextureMatrix;
          public uvRespectAnchor: boolean;

          public clampMargin: number;
          public tileScale: Point | ObservablePoint;
          public tilePosition: Point | ObservablePoint;

          public width: number;
          public height: number;
          protected _width: number;
          protected _height: number;
          protected _canvasPattern: CanvasPattern;
          constructor(texture: Texture, width?: number, height?: number);

          public multiplyUvs(uvs: Float32Array, out: Float32Array): Float32Array;
          public getLocalBounds(rect?: Rectangle): Rectangle;
          public containsPoint(point: Point): boolean;
          public destroy(options?: DestroyOptions | boolean): void;

          protected _onTextureUpdate(): void;
          protected _renderWebGL(renderer: WebGLRenderer): void;
          protected _renderCanvas(renderer: CanvasRenderer): void;
          protected _calculateBounds(): void;
      }
      export class TilingSpriteRenderer extends ObjectRenderer {
          constructor(renderer: WebGLRenderer);

          public render(ts: TilingSprite): void;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////// FILTERS///////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace filters {
      export class FXAAFilter extends Filter<{}> {}
      export class BlurFilter extends Filter<{}> {

          public blurXFilter: BlurXFilter;
          public blurYFilter: BlurYFilter;
          public resolution: number;
          public padding: number;
          public passes: number;
          public blur: number;
          public blurX: number;
          public blurY: number;
          public quality: number;
          public blendMode: number;
          constructor(strength?: number, quality?: number, resolution?: number, kernelSize?: number);
      }
      interface BlurXFilterUniforms {
          strength: number;
      }
      export class BlurXFilter extends Filter<BlurXFilterUniforms> {

          public quality: number;
          public passes: number;
          public resolution: number;
          public strength: number;
          public firstRun: boolean;
          public blur: number;

          protected _quality: number;
          constructor(strength?: number, quality?: number, resolution?: number, kernelSize?: number);
      }
      interface BlurYFilterUniforms {
          strength: number;
      }
      export class BlurYFilter extends Filter<BlurYFilterUniforms> {

          public quality: number;
          public passes: number;
          public resolution: number;
          public strength: number;
          public firstRun: boolean;
          public blur: number;

          protected _quality: number;
          constructor(strength?: number, quality?: number, resolution?: number, kernelSize?: number);
      }
      interface ColorMatrixFilterUniforms {
          m: Matrix;
          uAlpha: number;
      }
      export class ColorMatrixFilter extends Filter<ColorMatrixFilterUniforms> {

          public matrix: number[];
          public alpha: number;
          constructor();

          public brightness(b: number, multiply?: boolean): void;
          public greyscale(scale: number, multiply?: boolean): void;
          public blackAndWhite(multiply?: boolean): void;
          public hue(rotation: number, multiply?: boolean): void;
          public contrast(amount: number, multiply?: boolean): void;
          public saturate(amount: number, multiply?: boolean): void;
          public desaturate(multiply?: boolean): void;
          public negative(multiply?: boolean): void;
          public sepia(multiply?: boolean): void;
          public technicolor(multiply?: boolean): void;
          public polaroid(multiply?: boolean): void;
          public toBGR(multiply?: boolean): void;
          public kodachrome(multiply?: boolean): void;
          public browni(multiply?: boolean): void;
          public vintage(multiply?: boolean): void;
          public colorTone(desaturation: number, toned: number, lightColor: string, darkColor: string, multiply?: boolean): void;
          public night(intensity: number, multiply?: boolean): void;
          public predator(amount: number, multiply?: boolean): void;
          public lsd(multiply?: boolean): void;
          public reset(): void;

          protected _loadMatrix(matrix: number[], multiply?: boolean): void;
          protected _multiply(out: number[], a: number[], b: number[]): void;
          protected _colorMatrix(matrix: number[]): void;
      }
      interface DisplacementFilterUniforms {
          mapSampler: Texture;
          filterMatrix: Matrix;
          scale: Point;
      }
      export class DisplacementFilter extends Filter<DisplacementFilterUniforms> {

          public scale: Point;
          public map: Texture;
          constructor(sprite: Sprite, scale?: number);
      }
      export class AlphaFilter extends Filter<{}> {

          public alpha: number;
          public glShaderKey: number;
          constructor(alpha?: number);
      }

      // pixi-filters.d.ts todo
      // https://github.com/pixijs/pixi-filters/
      interface NoiseFilterUniforms {
          uNoise: number;
          uSeed: number;
      }
      export class NoiseFilter extends Filter<NoiseFilterUniforms> {

          public noise: number;
          public seed: number;
          constructor(noise?: number, seed?: number);
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////// INTERACTION///////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace interaction {
      export interface InteractiveTarget {
          interactive: boolean;
          interactiveChildren: boolean;
          hitArea: PIXI.Rectangle | PIXI.Circle | PIXI.Ellipse | PIXI.Polygon | PIXI.RoundedRectangle | PIXI.HitArea;
          buttonMode: boolean;
          cursor: string;
          trackedPointers: { [key: number]: InteractionTrackingData };

          // depricated
          defaultCursor: string;
      }
      export interface InteractionTrackingData {
          readonly pointerId: number;
          flags: number;
          none: number;
          over: boolean;
          rightDown: boolean;
          leftDown: boolean;
      }
      export interface InteractionEvent {
          stopped: boolean;
          target: DisplayObject;
          currentTarget: DisplayObject;
          type: string;
          data: InteractionData;
          stopPropagation(): void;
          reset(): void;
      }
      export class InteractionData {
          public global: Point;
          public target: DisplayObject;
          public originalEvent: MouseEvent | TouchEvent | PointerEvent;
          public identifier: number;
          public isPrimary: boolean;
          public button: number;
          public buttons: number;
          public width: number;
          public height: number;
          public tiltX: number;
          public tiltY: number;
          public pointerType: string;
          public pressure: number;
          public rotationAngle: number;
          public twist: number;
          public tangentialPressure: number;

          public readonly pointerID: number;

          public copyEvent(event: Touch | MouseEvent | PointerEvent): void;
          public reset(): void;

          public getLocalPosition(displayObject: DisplayObject, point?: Point, globalPos?: Point): Point;
      }

      type InteractionPointerEvents =
          | "pointerdown"
          | "pointercancel"
          | "pointerup"
          | "pointertap"
          | "pointerupoutside"
          | "pointermove"
          | "pointerover"
          | "pointerout";
      type InteractionTouchEvents = "touchstart" | "touchcancel" | "touchend" | "touchendoutside" | "touchmove" | "tap";
      type InteractionMouseEvents =
          | "rightdown"
          | "mousedown"
          | "rightup"
          | "mouseup"
          | "rightclick"
          | "click"
          | "rightupoutside"
          | "mouseupoutside"
          | "mousemove"
          | "mouseover"
          | "mouseout"
          | "mouseover";
      type InteractionPixiEvents = "added" | "removed";
      type InteractionEventTypes = InteractionPointerEvents | InteractionTouchEvents | InteractionMouseEvents | InteractionPixiEvents;

      export interface InteractionManagerOptions {
          autoPreventDefault?: boolean;
          interactionFrequency?: number;
      }
      export class InteractionManager extends utils.EventEmitter {

          public renderer: SystemRenderer;
          public autoPreventDefault: boolean;
          public interactionFrequency: number;
          public mouse: InteractionData;
          public activeInteractionData: { [key: number]: InteractionData };
          public interactionDataPool: InteractionData[];
          public eventData: InteractionEvent;
          public moveWhenInside: boolean;
          public eventsAdded: boolean;
          public readonly supportsTouchEvents: boolean;
          public readonly supportsPointerEvents: boolean;
          public cursorStyles: {
              default: string;
              pointer: string;
          };
          public currentCursorMode: string;
          public cursor: string;
          public resolution: number;

          // depricated
          public defaultCursorStyle: string;
          public currentCursorStyle: string;
          protected interactionDOMElement: HTMLElement;
          protected mouseOverRenderer: boolean;
          protected onPointerUp: (event: PointerEvent) => void;
          protected processPointerUp: (
              interactionEvent: InteractionEvent,
              displayObject: Container | PIXI.Sprite | PIXI.extras.TilingSprite,
              hit: boolean,
          ) => void;
          protected onPointerCancel: (event: PointerEvent) => void;
          protected processPointerCancel: (
              interactionEvent: InteractionEvent,
              displayObject: PIXI.Container | PIXI.Sprite | PIXI.extras.TilingSprite,
          ) => void;
          protected onPointerDown: (event: PointerEvent) => void;
          protected processPointerDown: (
              interactionEvent: InteractionEvent,
              displayObject: PIXI.Container | PIXI.Sprite | PIXI.extras.TilingSprite,
              hit: boolean,
          ) => void;
          protected onPointerMove: (event: PointerEvent) => void;
          protected processPointerMove: (
              interactionEvent: InteractionEvent,
              displayObject: PIXI.Container | PIXI.Sprite | PIXI.extras.TilingSprite,
              hit: boolean,
          ) => void;
          protected onPointerOut: (event: PointerEvent) => void;
          protected processPointerOverOut: (
              interactionEvent: InteractionEvent,
              displayObject: PIXI.Container | PIXI.Sprite | PIXI.extras.TilingSprite,
              hit: boolean,
          ) => void;
          protected onPointerOver: (event: PointerEvent) => void;
          protected _tempPoint: Point;
          constructor(renderer: CanvasRenderer | WebGLRenderer | SystemRenderer, options?: InteractionManagerOptions);
          public hitTest(globalPoint: Point, root?: Container): DisplayObject;
          public setTargetElement(element: HTMLCanvasElement, resolution?: number): void;
          public update(deltaTime?: number): void;
          public setCursorMode(mode: string): void;
          public mapPositionToPoint(point: Point, x: number, y: number): void;
          public destroy(): void;
          protected addEvents(): void;
          protected removeEvents(): void;
          protected dispatchEvent(displayObject: Container | Sprite | extras.TilingSprite, eventString: string, eventData: any): void;
          // tslint:disable-next-line:ban-types forbidden-types
          protected processInteractive(
              interactionEvent: InteractionEvent,
              displayObject: PIXI.Container | PIXI.Sprite | PIXI.extras.TilingSprite,
              func?: Function,
              hitTest?: boolean,
              interactive?: boolean,
          ): boolean;
          // tslint:disable-next-line:ban-types forbidden-types
          protected onPointerComplete(originalEvent: PointerEvent, cancelled: boolean, func: Function): void;
          protected getInteractionDataForPointerId(pointerId: number): InteractionData;
          protected releaseInteractionDataForPointerId(event: PointerEvent): void;
          protected configureInteractionEventForDOMEvent(
              interactionEvent: InteractionEvent,
              pointerEvent: PointerEvent,
              interactionData: InteractionData,
          ): InteractionEvent;
          protected normalizeToPointerData(event: TouchEvent | MouseEvent | PointerEvent): PointerEvent[];
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  /////////////////////////////// LOADER/////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  // pixi loader extends
  // https://github.com/englercj/resource-loader/
  // 2.1.1

  class MiniSignalBinding {

      // tslint:disable-next-line:ban-types forbidden-types
      protected _fn: Function;
      protected _once: boolean;
      protected _thisArg: any;
      protected _next: MiniSignalBinding;
      protected _prev: MiniSignalBinding;
      protected _owner: MiniSignal;
      // tslint:disable-next-line:ban-types forbidden-types
      constructor(fn: Function, once?: boolean, thisArg?: any);

      public detach(): boolean;
  }
  class MiniSignal {

      protected _head: MiniSignalBinding;
      protected _tail: MiniSignalBinding;
      constructor();

      public handlers(exists?: boolean): MiniSignalBinding[] | boolean;
      public handlers(exists?: true): boolean;
      public handlers(exists?: false): MiniSignalBinding[];

      public has(node: MiniSignalBinding): boolean;
      public dispatch(): boolean;
      // tslint:disable-next-line:ban-types forbidden-types
      public add(fn: Function, thisArg?: any): any;
      // tslint:disable-next-line:ban-types forbidden-types
      public once(fn: Function, thisArg?: any): any;
      public detach(node: MiniSignalBinding): MiniSignal;
      public detachAll(): MiniSignal;
  }

  export namespace loaders {
      export interface LoaderOptions {
          crossOrigin?: boolean | string;
          loadType?: number;
          xhrType?: string;
          metaData?: {
              loadElement?: HTMLImageElement | HTMLAudioElement | HTMLVideoElement;
              skipSource?: boolean;
              mimeType?: string | string[];
          };
      }
      export interface ResourceDictionary {
          [index: string]: PIXI.loaders.Resource;
      }

      // As of ResourceLoader v2 we no longer require EventEmitter
      // However, for depreciation reasons, it remains.
      export class Loader extends utils.EventEmitter {

          // below this line is the original non-pixi loader

          public static Resource: any;
          public static async: any;
          public static base64: any;
          // pixi overrides here

          // tslint:disable-next-line:ban-types forbidden-types
          public static addPixiMiddleware(fn: Function): void;

          public baseUrl: string;
          public progress: number;
          public loading: boolean;
          public defaultQueryString: string;

          public resources: ResourceDictionary;

          public onProgress: MiniSignal;
          public onError: MiniSignal;
          public onLoad: MiniSignal;
          public onStart: MiniSignal;
          public onComplete: MiniSignal;

          public concurrency: number;

          // tslint:disable-next-line:ban-types forbidden-types
          protected _beforeMiddleware: Function[];
          // tslint:disable-next-line:ban-types forbidden-types
          protected _afterMiddleware: Function[];
          protected _resourcesParsing: Resource[];
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundLoadResource: (r: Resource, d: Function) => void;
          protected _queue: any;

          constructor(baseUrl?: string, concurrency?: number);

          public add(...params: any[]): this;
          // tslint:disable-next-line:ban-types forbidden-types
          public add(name: string, url: string, options?: LoaderOptions, cb?: Function): this;
          // tslint:disable-next-line:ban-types forbidden-types
          public add(obj: string | any | any[], options?: LoaderOptions, cb?: Function): this;

          // tslint:disable-next-line:ban-types forbidden-types
          public pre(fn: Function): this;
          // tslint:disable-next-line:ban-types forbidden-types
          public use(fn: Function): this;
          public reset(): this;
          // tslint:disable-next-line:ban-types forbidden-types
          public load(cb?: Function): this;

          public destroy(): void;

          // depreciation

          public on(event: "complete", fn: (loader: loaders.Loader, object: any) => void, context?: any): this;
          public on(event: "error", fn: (error: Error, loader: loaders.Loader, resource: Resource) => void, context?: any): this;
          public on(event: "load" | "progress", fn: (loader: loaders.Loader, resource: Resource) => void, context?: any): this;
          public on(event: "start", fn: (loader: loaders.Loader) => void, context?: any): this;

          public once(event: "complete", fn: (loader: loaders.Loader, object: any) => void, context?: any): this;
          public once(event: "error", fn: (error: Error, loader: loaders.Loader, resource: Resource) => void, context?: any): this;
          public once(event: "load" | "progress", fn: (loader: loaders.Loader, resource: Resource) => void, context?: any): this;
          public once(event: "start", fn: (loader: loaders.Loader) => void, context?: any): this;
          // tslint:disable-next-line:ban-types forbidden-types
          public off(event: "complete" | "error" | "load" | "progress" | "start" | string, fn?: Function, context?: any): this;

          protected _prepareUrl(url: string): string;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _loadResource(resource: Resource, dequeue: Function): void;
          protected _onStart(): void;
          protected _onComplete(): void;
          protected _onLoad(resource: Resource): void;
      }
      export interface TextureDictionary {
          [index: string]: PIXI.Texture;
      }
      export class Resource {

          public static STATUS_FLAGS: {
              NONE: number;
              DATA_URL: number;
              COMPLETE: number;
              LOADING: number;
          };

          public static TYPE: {
              UNKNOWN: number;
              JSON: number;
              XML: number;
              IMAGE: number;
              AUDIO: number;
              VIDEO: number;
              TEXT: number;
          };

          public static LOAD_TYPE: {
              XHR: number;
              IMAGE: number;
              AUDIO: number;
              VIDEO: number;
          };

          public static XHR_RESPONSE_TYPE: {
              DEFAULT: string;
              BUFFER: string;
              BLOB: string;
              DOCUMENT: string;
              JSON: string;
              TEXT: string;
          };

          public static EMPTY_GIF: string;
          public static setExtensionLoadType(extname: string, loadType: number): void;
          public static setExtensionXhrType(extname: string, xhrType: string): void;

          public name: string;
          public url: string;
          public extension: string;
          public data: any;
          public crossOrigin: boolean | string;
          public loadType: number;
          public xhrType: string;
          public metadata: any;
          public error: Error;
          public xhr: XMLHttpRequest | null;
          public children: Resource[];
          public type: number;
          public progressChunk: number;

          public onStart: MiniSignal;
          public onProgress: MiniSignal;
          public onComplete: MiniSignal;
          public onAfterMiddleware: MiniSignal;

          public isDataUrl: boolean;
          public isComplete: boolean;
          public isLoading: boolean;

          public texture: Texture;
          public spineAtlas: any;
          public spineData: any;
          public textures?: TextureDictionary;
          public spritesheet?: Spritesheet;

          protected _flags: number;

          // tslint:disable-next-line:ban-types forbidden-types
          protected _dequeue: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _onLoadBinding: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundComplete: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundOnError: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundOnProgress: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundXhrOnError: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundXhrOnAbort: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundXhrOnLoad: Function;
          // tslint:disable-next-line:ban-types forbidden-types
          protected _boundXdrOnTimeout: Function;

          constructor(name: string, url: string | string[], options?: LoaderOptions);
          public complete(): void;
          public abort(message?: string): void;
          // tslint:disable-next-line:ban-types forbidden-types
          public load(cb?: Function): void;

          protected _hasFlag(flag: number): boolean;
          protected _setFlag(flag: number, value: boolean): void;
          protected _loadElement(type: string): void;
          protected _loadSourceElement(type: string): void;
          protected _loadXhr(): void;
          protected _loadXdr(): void;
          protected _createSource(type: string, url: string, mime?: string): HTMLSourceElement;
          protected _onError(event?: any): void;
          protected _onProgress(event?: any): void;
          protected _xhrOnError(): void;
          protected _xhrOnAbort(): void;
          protected _xdrOnTimeout(): void;
          protected _xhrOnLoad(): void;
          protected _determineCrossOrigin(url: string, loc: any): string;
          protected _determineXhrType(): number;
          protected _determineLoadType(): number;
          protected _getExtension(): string;
          protected _getMimeXhrType(type: number): string;
      }
      const shared: Loader;
  }

  //////////////////////////////////////////////////////////////////////////////
  /////////////////////////////// MESH///////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace mesh {
      export class Mesh extends Container {

          public static DRAW_MODES: {
              TRIANGLE_MESH: number;
              TRIANGLES: number;
          };
          public uvs: Float32Array;
          public vertices: Float32Array;
          public indices: Uint16Array;
          public dirty: number;
          public indexDirty: number;
          public vertexDirty: number;
          public autoUpdate: boolean;
          public dirtyVertex: boolean;
          public blendMode: number;
          public pluginName: string;
          public canvasPadding: number;
          public drawMode: number;
          public texture: Texture;
          public tintRgb: Float32Array;
          public uploadUvTransform: boolean;
          public tint: number;

          protected _texture: Texture;
          protected _geometryVersion: number;
          protected _glDatas: { [n: number]: any };
          protected _uvTransform: TextureMatrix;
          constructor(texture: Texture, vertices?: Float32Array, uvs?: Float32Array, indices?: Uint16Array, drawMode?: number);
          public multiplyUvs(): void;
          public refresh(forceUpdate?: boolean): void;
          public containsPoint(point: Point): boolean;
          protected _refresh(): void;
          protected _renderWebGL(renderer: WebGLRenderer): void;
          protected _renderCanvas(renderer: CanvasRenderer): void;
          protected _onTextureUpdate(): void;
          protected _calculateBounds(): void;
      }

      export class CanvasMeshRenderer {

          public renderer: CanvasRenderer;
          constructor(renderer: CanvasRenderer);

          public render(mesh: Mesh): void;

          public destroy(): void;
          protected _renderTriangleMesh(mesh: Mesh): void;
          protected _renderTriangles(mesh: Mesh): void;
          protected _renderDrawTriangle(mesh: Mesh, index0: number, index1: number, index2: number): void;
          protected renderMeshFlat(mesh: Mesh): void;
      }

      export class MeshRenderer extends ObjectRenderer {

          public shader: Shader;
          constructor(renderer: WebGLRenderer);
          public render(mesh: Mesh): void;
      }

      export class Plane extends Mesh {
          public verticesX: number;
          public verticesY: number;
          public drawMode: number;
          protected _ready: boolean;
          constructor(texture: Texture, verticesX?: number, verticesY?: number);

          public refresh(): void;

          protected _onTexureUpdate(): void;
      }

      export class NineSlicePlane extends Plane {

          public width: number;
          public height: number;
          public leftWidth: number;
          public rightWidth: number;
          public topHeight: number;
          public bottomHeight: number;

          protected _leftWidth: number;
          protected _rightWidth: number;
          protected _topHeight: number;
          protected _bottomHeight: number;
          protected _height: number;
          protected _width: number;
          protected _origHeight: number;
          protected _origWidth: number;
          protected _uvh: number;
          protected _uvw: number;
          constructor(texture: Texture, leftWidth?: number, topHeight?: number, rightWidth?: number, bottomHeight?: number);

          public updateHorizontalVertices(): void;
          public updateVerticalVertices(): void;
          protected drawSegment(
              context: CanvasRenderingContext2D | WebGLRenderingContext,
              textureSource: any,
              w: number,
              h: number,
              x1: number,
              y1: number,
              x2: number,
              y2: number,
          ): void;
          protected _refresh(): void;
      }

      export class Rope extends Mesh {

          public points: Point[];
          public colors: number[];
          public autoUpdate: boolean;
          constructor(texture: Texture, points: Point[]);

          public refreshVertices(): void;
          protected _refresh(): void;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  ///////////////////////////// PARTICLES////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace particles {
      export interface ParticleContainerProperties {
          /**
           * DEPRECIATED - Use `vertices`
           */
          scale?: boolean;
          vertices?: boolean;
          position?: boolean;
          rotation?: boolean;
          uvs?: boolean;
          tint?: boolean;
          alpha?: boolean;
      }
      export class ParticleContainer extends Container {
          public tint: number;
          public interactiveChildren: boolean;
          public blendMode: number;
          public autoResize: boolean;
          public roundPixels: boolean;
          public baseTexture: BaseTexture;

          protected _tint: number;
          protected tintRgb: number | any[];
          protected _properties: boolean[];
          protected _maxSize: number;
          protected _batchSize: number;
          protected _glBuffers: { [n: number]: WebGLBuffer };
          protected _bufferUpdateIDs: number[];
          protected _updateID: number;
          protected onChildrenChange: (smallestChildIndex?: number) => void;
          constructor(maxSize?: number, properties?: ParticleContainerProperties, batchSize?: number, autoResize?: boolean);

          public setProperties(properties: ParticleContainerProperties): void;

          public destroy(options?: DestroyOptions | boolean): void;
      }
      export class ParticleBuffer {

          public gl: WebGLRenderingContext;
          public size: number;
          public dynamicProperties: any[];
          public staticProperties: any[];
          public staticStride: number;
          public staticBuffer: any;
          public staticData: any;
          public staticDataUint32: any;
          public dynamicStride: number;
          public dynamicBuffer: any;
          public dynamicData: any;
          public dynamicDataUint32: any;

          protected _updateID: number;
          constructor(gl: WebGLRenderingContext, properties: any, dynamicPropertyFlags: any[], size: number);

          public destroy(): void;
      }
      export interface ParticleRendererProperty {
          attribute: number;
          size: number;
          uploadFunction: (
              children: PIXI.DisplayObject[],
              startIndex: number,
              amount: number,
              array: number[],
              stride: number,
              offset: number,
          ) => void;
          unsignedByte: any;
          offset: number;
      }
      export class ParticleRenderer extends ObjectRenderer {

          public shader: glCore.GLShader;
          public indexBuffer: WebGLBuffer;
          public properties: ParticleRendererProperty[];

          public indices: Uint16Array;
          protected tempMatrix: Matrix;
          constructor(renderer: WebGLRenderer);

          public start(): void;
          public generateBuffers(container: ParticleContainer): ParticleBuffer[];
          public uploadVertices(
              children: DisplayObject[],
              startIndex: number,
              amount: number,
              array: number[],
              stride: number,
              offset: number,
          ): void;
          public uploadPosition(
              children: DisplayObject[],
              startIndex: number,
              amount: number,
              array: number[],
              stride: number,
              offset: number,
          ): void;
          public uploadRotation(
              children: DisplayObject[],
              startIndex: number,
              amount: number,
              array: number[],
              stride: number,
              offset: number,
          ): void;
          public uploadUvs(children: DisplayObject[], startIndex: number, amount: number, array: number[], stride: number, offset: number): void;
          public uploadTint(
              children: DisplayObject[],
              startIndex: number,
              amount: number,
              array: number[],
              stride: number,
              offset: number,
          ): void;
          public uploadAlpha(
              children: DisplayObject[],
              startIndex: number,
              amount: number,
              array: number[],
              stride: number,
              offset: number,
          ): void;
          public destroy(): void;
          protected _generateOneMoreBuffer(container: ParticleContainer): ParticleBuffer;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  //////////////////////////// PREPARE///////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export namespace prepare {
      type AddHook = (item: any, queue: any[]) => boolean;
      type UploadHook<UploadHookSource> = (prepare: UploadHookSource, item: any) => boolean;

      export abstract class BasePrepare<UploadHookSource> {

          public limiter: CountLimiter | TimeLimiter;
          protected renderer: SystemRenderer;
          protected uploadHookHelper: UploadHookSource;
          protected queue: any[];
          protected addHooks: AddHook[];
          protected uploadHooks: Array<UploadHook<UploadHookSource>>;
          // tslint:disable-next-line:ban-types forbidden-types
          protected completes: Function[];
          protected ticking: boolean;
          protected delayedTick: () => void;
          constructor(renderer: SystemRenderer);

          // tslint:disable-next-line:ban-types forbidden-types
          public upload(item: Function | DisplayObject | Container | BaseTexture | Texture | Graphics | Text | any, done?: () => void): void;
          public registerFindHook(addHook: AddHook): this;
          public registerUploadHook(uploadHook: UploadHook<UploadHookSource>): this;
          public add(item: PIXI.DisplayObject | PIXI.Container | PIXI.BaseTexture | PIXI.Texture | PIXI.Graphics | PIXI.Text | any): this;
          public destroy(): void;
          protected tick(): void;
          protected prepareItems(): void;
          protected findMultipleBaseTextures(item: PIXI.DisplayObject, queue: any[]): boolean;
          protected findBaseTexture(item: PIXI.DisplayObject, queue: any[]): boolean;
          protected findTexture(item: PIXI.DisplayObject, queue: any[]): boolean;
      }
      export class CanvasPrepare extends BasePrepare<CanvasPrepare> {

          protected canvas: HTMLCanvasElement;
          protected ctx: CanvasRenderingContext2D;
          constructor(renderer: CanvasRenderer);
      }
      export class WebGLPrepare extends BasePrepare<WebGLRenderer> {
          constructor(renderer: WebGLRenderer);
      }
      export class CountLimiter {

          protected maxItemsPerFrame: number;
          protected itemsLeft: number;
          constructor(maxItemsPerFrame: number);

          public beginFrame(): void;
          public allowedToUpload(): boolean;
      }
      export class TimeLimiter {

          protected maxMilliseconds: number;
          protected frameStart: number;
          constructor(maxMilliseconds: number);

          public beginFrame(): void;
          public allowedToUpload(): boolean;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  ///////////////////////////// pixi-gl-core/////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////
  // pixi-gl-core 1.1.4 https://github.com/pixijs/pixi-gl-core
  // sharedArrayBuffer as a type is not available yet.
  // need to fully define what an `Attrib` is.
  export namespace glCore {
      export interface ContextOptions {
          /**
           * Boolean that indicates if the canvas contains an alpha buffer.
           */
          alpha?: boolean;
          /**
           * Boolean that indicates that the drawing buffer has a depth buffer of at least 16 bits.
           */
          depth?: boolean;
          /**
           * Boolean that indicates that the drawing buffer has a stencil buffer of at least 8 bits.
           */
          stencil?: boolean;
          /**
           * Boolean that indicates whether or not to perform anti-aliasing.
           */
          antialias?: boolean;
          /**
           * Boolean that indicates that the page compositor will assume the drawing buffer contains colors with pre-multiplied alpha.
           */
          premultipliedAlpha?: boolean;
          /**
           * If the value is true the buffers will not be cleared and will preserve their values until cleared or overwritten by the author.
           */
          preserveDrawingBuffer?: boolean;
          /**
           *  Boolean that indicates if a context will be created if the system performance is low.
           */
          failIfMajorPerformanceCaveat?: boolean;
      }
      export function createContext(view: HTMLCanvasElement, options?: ContextOptions): WebGLRenderingContext;
      export function setVertexAttribArrays(
          gl: WebGLRenderingContext,
          attribs: Attrib[],
          state?: WebGLState,
      ): WebGLRenderingContext | undefined;
      export class GLBuffer {

          public static createVertexBuffer(gl: WebGLRenderingContext, data: ArrayBuffer | ArrayBufferView | any, drawType: number): GLBuffer;
          public static createIndexBuffer(gl: WebGLRenderingContext, data: ArrayBuffer | ArrayBufferView | any, drawType: number): GLBuffer;
          public static create(gl: WebGLRenderingContext, type: number, data: ArrayBuffer | ArrayBufferView | any, drawType: number): GLBuffer;
          public gl: WebGLRenderingContext;
          public buffer: WebGLBuffer;
          public type: number;
          public drawType: number;
          public data: ArrayBuffer | ArrayBufferView | any;

          protected _updateID?: number;
          constructor(gl: WebGLRenderingContext, type: number, data: ArrayBuffer | ArrayBufferView | any, drawType: number);

          public upload(data?: ArrayBuffer | ArrayBufferView | any, offset?: number, dontBind?: boolean): void;
          public bind(): void;

          public destroy(): void;
      }
      export class GLFramebuffer {

          public static createRGBA(
              gl: WebGLRenderingContext,
              width: number,
              height: number,
              data: ArrayBuffer | ArrayBufferView | any,
          ): GLFramebuffer;
          public static createFloat32(
              gl: WebGLRenderingContext,
              width: number,
              height: number,
              data: ArrayBuffer | ArrayBufferView | any,
          ): GLFramebuffer;

          public gl: WebGLRenderingContext;
          public frameBuffer: WebGLFramebuffer;
          public stencil: WebGLRenderbuffer;
          public texture: GLTexture;
          public width: number;
          public height: number;
          constructor(gl: WebGLRenderingContext, width: number, height: number);

          public enableTexture(texture: GLTexture): void;
          public enableStencil(): void;
          public clear(r: number, g: number, b: number, a: number): void;
          public bind(): void;
          public unbind(): void;
          public resize(width: number, height: number): void;
          public destroy(): void;
      }
      export class GLShader {

          public gl: WebGLRenderingContext;
          public program?: WebGLProgram | null;
          public uniformData: any;
          public uniforms: any;
          public attributes: any;
          constructor(
              gl: WebGLRenderingContext,
              vertexSrc: string | string[],
              fragmentSrc: string | string[],
              precision?: string,
              attributeLocations?: { [key: string]: number },
          );

          public bind(): this;
          public destroy(): void;
      }
      export class GLTexture {

          public static fromSource(
              gl: WebGLRenderingContext,
              source: HTMLImageElement | ImageData | HTMLVideoElement | HTMLCanvasElement,
              premultipleAlpha?: boolean,
          ): GLTexture;
          public static fromData(gl: WebGLRenderingContext, data: number[], width: number, height: number): GLTexture;

          public gl: WebGLRenderingContext;
          public texture: WebGLTexture;
          public mipmap: boolean;
          public premultiplyAlpha: boolean;
          public width: number;
          public height: number;
          public format: number;
          public type: number;
          constructor(gl: WebGLRenderingContext, width?: number, height?: number, format?: number, type?: number);

          public upload(source: HTMLImageElement | ImageData | HTMLVideoElement | HTMLCanvasElement): void;
          public uploadData(data: ArrayBuffer | ArrayBufferView, width: number, height: number): void;
          public bind(location?: number): void;
          public unbind(): void;
          public minFilter(linear: boolean): void;
          public magFilter(linear: boolean): void;
          public enableMipmap(): void;
          public enableLinearScaling(): void;
          public enableNearestScaling(): void;
          public enableWrapClamp(): void;
          public enableWrapRepeat(): void;
          public enableWrapMirrorRepeat(): void;
          public destroy(): void;
      }
      export interface Attrib {
          attribute: {
              location: number;
              size: number;
          };
          normalized: boolean;
          stride: number;
          start: number;
          buffer: ArrayBuffer;
      }
      export interface WebGLRenderingContextAttribute {
          buffer: WebGLBuffer;
          attribute: any;
          type: number;
          normalized: boolean;
          stride: number;
          start: number;
      }
      export interface AttribState {
          tempAttribState: Attrib[];
          attribState: Attrib[];
      }

      export class VertexArrayObject {
          public static FORCE_NATIVE: boolean;
          public gl: WebGLRenderingContext;
          public attributes: Attrib[];
          public indexBuffer: GLBuffer;
          public dirty: boolean;

          protected nativeVaoExtension: any;
          protected nativeState: AttribState;
          protected nativeVao: VertexArrayObject;

          constructor(gl: WebGLRenderingContext, state?: WebGLState);

          public bind(): this;
          public unbind(): this;
          public activate(): this;
          public addAttribute(buffer: GLBuffer, attribute: Attrib, type?: number, normalized?: boolean, stride?: number, start?: number): this;
          public addIndex(buffer: GLBuffer, options?: any): this;
          public clear(): this;
          public draw(type: number, size?: number, start?: number): this;
          public destroy(): void;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  /////////////////////////////// UTILS//////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////

  export interface DecomposedDataUri {
      mediaType: string;
      subType: string;
      encoding: string;
      data: any;
  }

  export namespace utils {
      export function uid(): number;
      export function hex2rgb(hex: number, out?: number[]): number[];
      export function hex2string(hex: number): string;
      export function rgb2hex(rgb: number[]): number;
      export function canUseNewCanvasBlendModes(): boolean;
      export function getResolutionOfUrl(url: string, defaultValue?: number): number;
      export function getSvgSize(svgString: string): any;
      export function decomposeDataUri(dataUri: string): DecomposedDataUri | void;
      export function getUrlFileExtension(url: string): string | void;
      export function sayHello(type: string): void;
      export function skipHello(): void;
      export function isWebGLSupported(): boolean;
      export function sign(n: number): number;
      export function removeItems<T>(arr: T[], startIdx: number, removeCount: number): void;
      export function correctBlendMode(blendMode: number, premultiplied: boolean): number;
      export function premultiplyTint(tint: number, alpha: number): number;
      export function premultiplyRgba(
          rgb: Float32Array | number[],
          alpha: number,
          out?: Float32Array,
          premultiply?: boolean,
      ): Float32Array;
      export function premultiplyTintToRgba(tint: number, alpha: number, out?: Float32Array, premultiply?: boolean): Float32Array;
      export function clearTextureCache(): void;
      export function destroyTextureCache(): void;
      export const premultiplyBlendMode: number[][];
      export const TextureCache: any;
      export const BaseTextureCache: any;

      // https://github.com/kaimallea/isMobile
      export namespace isMobile {
          export const apple: {
              phone: boolean;
              ipod: boolean;
              tablet: boolean;
              device: boolean;
          };
          export const android: {
              phone: boolean;
              tablet: boolean;
              device: boolean;
          };
          export const amazon: {
              phone: boolean;
              tablet: boolean;
              device: boolean;
          };
          export const windows: {
              phone: boolean;
              tablet: boolean;
              device: boolean;
          };
          export const seven_inch: boolean;
          export const other: {
              blackberry10: boolean;
              blackberry: boolean;
              opera: boolean;
              firefox: boolean;
              chrome: boolean;
              device: boolean;
          };
          export const any: boolean;
          export const phone: boolean;
          export const tablet: boolean;
      }

      // https://github.com/primus/eventemitter3
      export class EventEmitter {
          public static prefixed: string | boolean;

          public static EventEmitter: {
              prefixed: string | boolean;
              new (): EventEmitter;
          };

          /**
           * Minimal EventEmitter interface that is molded against the Node.js
           * EventEmitter interface.
           *
           * @constructor
           * @api public
           */
          constructor();

          /**
           * Return an array listing the events for which the emitter has registered listeners.
           *
           * @returns {(string | symbol)[]}
           */
          public eventNames(): Array<string | symbol>;

          /**
           * Return the listeners registered for a given event.
           *
           * @param {(string | symbol)} event The event name.
           * @returns {Function[]}
           */
          // tslint:disable-next-line:ban-types forbidden-types
          public listeners(event: string | symbol): Function[];

          /**
           * Check if there listeners for a given event.
           * If `exists` argument is not `true` lists listeners.
           *
           * @param {(string | symbol)} event The event name.
           * @param {boolean} exists Only check if there are listeners.
           * @returns {boolean}
           */
          public listeners(event: string | symbol, exists: boolean): boolean;

          /**
           * Calls each of the listeners registered for a given event.
           *
           * @param {(string | symbol)} event The event name.
           * @param {...*} args Arguments that are passed to registered listeners
           * @returns {boolean} `true` if the event had listeners, else `false`.
           */
          public emit(event: string | symbol, ...args: any[]): boolean;

          /**
           * Add a listener for a given event.
           *
           * @param {(string | symbol)} event The event name.
           * @param {Function} fn The listener function.
           * @param {*} [context=this] The context to invoke the listener with.
           * @returns {EventEmitter} `this`.
           */
          // tslint:disable-next-line:ban-types forbidden-types
          public on(event: string | symbol, fn: Function, context?: any): this;

          /**
           * Add a one-time listener for a given event.
           *
           * @param {(string | symbol)} event The event name.
           * @param {Function} fn The listener function.
           * @param {*} [context=this] The context to invoke the listener with.
           * @returns {EventEmitter} `this`.
           */
          // tslint:disable-next-line:ban-types forbidden-types
          public once(event: string | symbol, fn: Function, context?: any): this;

          /**
           * Remove the listeners of a given event.
           *
           * @param {(string | symbol)} event The event name.
           * @param {Function} fn Only remove the listeners that match this function.
           * @param {*} context Only remove the listeners that have this context.
           * @param {boolean} once Only remove one-time listeners.
           * @returns {EventEmitter} `this`.
           */
          // tslint:disable-next-line:ban-types forbidden-types
          public removeListener(event: string | symbol, fn?: Function, context?: any, once?: boolean): this;

          /**
           * Remove all listeners, or those of the specified event.
           *
           * @param {(string | symbol)} event The event name.
           * @returns {EventEmitter} `this`.
           */
          public removeAllListeners(event?: string | symbol): this;

          /**
           * Alias method for `removeListener`
           */
          // tslint:disable-next-line:ban-types forbidden-types
          public off(event: string | symbol, fn?: Function, context?: any, once?: boolean): this;

          /**
           * Alias method for `on`
           */
          // tslint:disable-next-line:ban-types forbidden-types
          public addListener(event: string | symbol, fn: Function, context?: any): this;

          /**
           * This function doesn't apply anymore.
           * @deprecated
           */
          public setMaxListeners(): this;
      }
  }

  //////////////////////////////////////////////////////////////////////////////
  ///////////////////////////// depreciation/////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////
  // not sure how to handle blendmodes scalemodes basetexturecache
  namespace core {
      /**
       * @class
       * @private
       * @name SpriteBatch
       * @memberof PIXI
       * @see PIXI.ParticleContainer
       * @throws {ReferenceError} SpriteBatch does not exist any more, please use the new ParticleContainer instead.
       * @deprecated since version 3.0.0
       */
      type SpriteBatch = ParticleContainer;

      /**
       * @class
       * @private
       * @name AssetLoader
       * @memberof PIXI
       * @see PIXI.loaders.Loader
       * @throws {ReferenceError} The loader system was overhauled in pixi v3, please see the new PIXI.loaders.Loader class.
       * @deprecated since version 3.0.0
       */
      type AssetLoader = loaders.Loader;

      /**
       * @class
       * @private
       * @name Stage
       * @memberof PIXI
       * @see PIXI.Container
       * @deprecated since version 3.0.0
       */
      type Stage = Container;

      /**
       * @class
       * @private
       * @name DisplayObjectContainer
       * @memberof PIXI
       * @see PIXI.Container
       * @deprecated since version 3.0.0
       */
      type DisplayObjectContainer = Container;

      /**
       * @class
       * @private
       * @name Strip
       * @memberof PIXI
       * @see PIXI.mesh.Mesh
       * @deprecated since version 3.0.0
       */
      type Strip = mesh.Mesh;

      /**
       * @class
       * @private
       * @name Rope
       * @memberof PIXI
       * @see PIXI.mesh.Rope
       * @deprecated since version 3.0.0
       */
      type Rope = mesh.Rope;

      /**
       * @class
       * @private
       * @name ParticleContainer
       * @memberof PIXI
       * @see PIXI.particles.ParticleContainer
       * @deprecated since version 4.0.0
       */
      type ParticleContainer = particles.ParticleContainer;

      /**
       * @class
       * @private
       * @name MovieClip
       * @memberof PIXI
       * @see PIXI.extras.MovieClip
       * @deprecated since version 3.0.0
       */
      type MovieClip = extras.AnimatedSprite;

      /**
       * @class
       * @private
       * @name TilingSprite
       * @memberof PIXI
       * @see PIXI.extras.TilingSprite
       * @deprecated since version 3.0.0
       */
      type TilingSprite = extras.TilingSprite;

      /**
       * @class
       * @private
       * @name BaseTextureCache
       * @memberof PIXI
       * @see PIXI.utils.BaseTextureCache
       * @deprecated since version 3.0.0
       */
      type BaseTextureCache = any;

      /**
       * @class
       * @private
       * @name BitmapText
       * @memberof PIXI
       * @see PIXI.extras.BitmapText
       * @deprecated since version 3.0.0
       */
      type BitmapText = extras.BitmapText;

      /**
       * @namespace
       * @private
       * @name math
       * @memberof PIXI
       * @see PIXI
       * @deprecated since version 3.0.6
       */
      type math = any;

      /**
       * @class
       * @private
       * @name PIXI.AbstractFilter
       * @see PIXI.Filter
       * @deprecated since version 3.0.6
       */
      type AbstractFilter<U extends object> = Filter<U>;

      /**
       * @class
       * @private
       * @name PIXI.TransformManual
       * @see PIXI.TransformBase
       * @deprecated since version 4.0.0
       */
      type TransformManual = TransformBase;

      /**
       * @static
       * @constant
       * @name PIXI.TARGET_FPMS
       * @see PIXI.settings.TARGET_FPMS
       * @deprecated since version 4.2.0
       */
      type TARGET_FPMS = number;

      /**
       * @static
       * @constant
       * @name PIXI.FILTER_RESOLUTION
       * @see PIXI.settings.FILTER_RESOLUTION
       * @deprecated since version 4.2.0
       */
      type FILTER_RESOLUTION = number;

      /**
       * @static
       * @constant
       * @name PIXI.RESOLUTION
       * @see PIXI.settings.RESOLUTION
       * @deprecated since version 4.2.0
       */
      type RESOLUTION = number;

      /**
       * @static
       * @constant
       * @name PIXI.MIPMAP_TEXTURES
       * @see PIXI.settings.MIPMAP_TEXTURES
       * @deprecated since version 4.2.0
       */
      type MIPMAP_TEXTURES = any;

      /**
       * @static
       * @constant
       * @name PIXI.SPRITE_BATCH_SIZE
       * @see PIXI.settings.SPRITE_BATCH_SIZE
       * @deprecated since version 4.2.0
       */
      type SPRITE_BATCH_SIZE = number;

      /**
       * @static
       * @constant
       * @name PIXI.SPRITE_MAX_TEXTURES
       * @see PIXI.settings.SPRITE_MAX_TEXTURES
       * @deprecated since version 4.2.0
       */
      type SPRITE_MAX_TEXTURES = number;

      /**
       * @static
       * @constant
       * @name PIXI.RETINA_PREFIX
       * @see PIXI.settings.RETINA_PREFIX
       * @deprecated since version 4.2.0
       */
      type RETINA_PREFIX = RegExp | string;

      /**
       * @static
       * @constant
       * @name PIXI.DEFAULT_RENDER_OPTIONS
       * @see PIXI.settings.RENDER_OPTIONS
       * @deprecated since version 4.2.0
       */
      type DEFAULT_RENDER_OPTIONS = number;

      /**
       * @static
       * @name PRECISION
       * @memberof PIXI.settings
       * @see PIXI.PRECISION
       * @deprecated since version 4.4.0
       */
      type PRECISION = string;
  }

  export namespace extras {
      /**
       * @class
       * @name MovieClip
       * @memberof PIXI.extras
       * @see PIXI.extras.AnimatedSprite
       * @deprecated since version 4.2.0
       */
      type MovieClip = extras.AnimatedSprite;

      /**
       * @class
       * @name TextureTransform
       * @memberof PIXI.extras
       * @see PIXI.TextureMatrix
       * @deprecated since version 4.6.0
       */
      type TextureTranform = TextureMatrix;
  }

  export namespace GroupD8 {
      /**
       * @method
       * @name PIXI.GroupD8.isSwapWidthHeight
       * @see PIXI.GroupD8.isVertical
       * @param {number} rotation - The number to check.
       * @returns {boolean} Whether or not the direction is vertical
       * @deprecated since version 4.6.0
       */
      export function isSwapWidthHeight(rotation: number): boolean;
  }

  export namespace filters {
      /**
       * @class
       * @private
       * @name PIXI.filters.VoidFilter
       * @see PIXI.filters.AlphaFilter
       * @deprecated since version 4.5.7
       */
      type VoidFilter = filters.AlphaFilter;
  }

  export namespace settings {
      /**
       * @static
       * @name PRECISION
       * @memberof PIXI.settings
       * @see PIXI.PRECISION
       * @deprecated since version 4.4.0
       */
      type PRECISION = number;
  }
}

declare namespace pixi {
  export const gl: typeof PIXI.glCore;
}

// tslint:disable-next-line:no-single-declare-module
declare module "pixi.js" {
  export = PIXI;
}
