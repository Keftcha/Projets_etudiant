import { GroupPeriod } from "./GroupPeriod"

class PeriodCreate {
    public begDate!: Date
    public endDate!: Date
    public Kind!: string
    public Groups!: GroupPeriod[]
    public idZone!: number

}
export { PeriodCreate }