class Event {
    public eventId: number
    public name: string
    public begDate: Date
    public endDate: Date
    public isOther: boolean

    constructor()
    constructor(eventId: number, name: string, begDate: Date, endDate: Date )
    constructor(eventId: number, name: string, begDate: Date, endDate: Date, isOther: boolean )
    constructor(eventId: number = 0, name: string = "", begDate = new Date(), endDate = new Date(), isOther = false) {
        this.eventId = eventId
        this.name = name
        this.begDate = new Date(begDate)
        this.endDate = new Date(endDate)
        this.isOther = isOther
    }

    public clone() {
        return new Event(this.eventId, this.name, this.begDate, this.endDate, this.isOther)
    }
}
export { Event }