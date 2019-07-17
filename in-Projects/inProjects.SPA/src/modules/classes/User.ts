class User {
    public firstName: string
    public lastName: string
    public email: string
    public password: string
    public groupName: string
    public emailSecondary: string = ""

    constructor()
    constructor(firstName: string, lastName: string)
    constructor(first: string, last: string, email: string, passwd: string, grp: string, sndemail: string)
    constructor(first: string = "", last: string = "", email: string = "", passwd: string = "", grp: string = "", sndemail: string = "") {
        this.firstName = first
        this.lastName = last
        this.email = email
        this.password = passwd
        this.groupName = grp
        this.emailSecondary = sndemail
    }

    clone(): User {return new User(this.firstName, this.lastName, this.email, this.password, this.groupName, this.emailSecondary)}

    // Set or Reset a User to default values
    public reset() {
        this.firstName = ""
        this.lastName = ""
        this.email = ""
        this.password = ""
    }

}
export { User }