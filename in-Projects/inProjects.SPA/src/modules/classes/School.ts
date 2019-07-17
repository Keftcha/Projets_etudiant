class School {
    public schoolId: number
    public name: string

    constructor()
    constructor(schoolId: number, name: string)
    constructor(schoolId: number = 0, name: string = "") {
        this.schoolId = schoolId
        this.name = name
    }
}
export { School }