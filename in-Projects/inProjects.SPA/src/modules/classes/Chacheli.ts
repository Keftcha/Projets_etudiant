class Chacheli {
    public forumNumber!: number
    public x!: number
    public y!: number
    public w!: number
    public h!: number
    public text!: string
    public available!: boolean
    public comp!: string
    public projectId!: number
    public classRoom!: string
    public name!: string

    constructor(id: number, name: string, x: number, y: number, w: number, h: number, text: string, a: boolean, comp: string, projectId: number, classRoom: string) {
        this.forumNumber = id
        this.x = x
        this.y =  y
        this.w =  w
        this.h =  h
        this.text = text
        this.available = a
        this.comp = comp
        this.projectId = projectId
        this.classRoom = classRoom
        this.name = name
    }
}

export { Chacheli }