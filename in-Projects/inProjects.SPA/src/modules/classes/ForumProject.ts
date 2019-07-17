class ForumProject {
    public name!: string
    public userId !: number[]
    public posX!: number
    public posY!: number
    public width!: number
    public height!: number
    public semester!: string
    public classRoom!: string
    public forumNumber!: number
    public projectId!: number

    constructor(name: string, posX: number, posY: number, width: number, height: number, semester: string, classroom: string, forumNumber: number, projectId: number) {
        this.name = name
        this.posX = posX
        this.posY = posY
        this.width = width
        this.height = height
        this.semester = semester
        this.classRoom = classroom
        this.forumNumber = forumNumber
        this.projectId = projectId
    }
}
export { ForumProject }