<template>
    <div class="maincal">
        <div class="calendar-parent">
            <calendar-view
                :events="events"
                :show-date="showDate"
                :enable-drag-drop="true"
                @drop-on-date="onDrop"
                @click-event="onClickEvent"
                class="theme-default holiday-us-traditional holiday-us-official">
                <calendar-view-header
                    slot="header"
                    slot-scope="t"
                    :header-props="t.headerProps"
                    @input="setShowDate" />
            </calendar-view>
         </div>
           <el-dialog :title='eventClicked.name' :visible.sync="outerVisible">

                 <el-date-picker  type="datetime" :clearable="false" v-model="eventClicked.begDate"  @change="changeDate(eventClicked.eventId)"></el-date-picker>   
                 <el-date-picker  type="datetime" :clearable="false" v-model="eventClicked.endDate"  @change="changeDate(eventClicked.eventId)"></el-date-picker>    
                
                <div slot="footer" class="dialog-footer">
                <el-button @click="outerVisible = false">Annuler</el-button>
                <el-button type="danger" @click="innerVisible = true">Supprimer l'évènements</el-button>
                </div>

                <el-dialog width="30%" title=" Etes-vous sur de vouloir supprimer ?" :visible.sync="innerVisible" append-to-body>
                    <el-button @click="innerVisible = false">Non</el-button>
                    <el-button type="danger" @click="DeleteEvent">Oui</el-button>
                </el-dialog>

            </el-dialog>
    </div>
</template>

<script lang="js">
import { CalendarView, CalendarViewHeader } from "vue-simple-calendar"
import {GetEventsSchool, UpdateEvents, CreateEvents, GetEventsType,DeleteEvent} from "../api/eventApi"
import {Event} from "../modules/classes/EventSchool"

	require("vue-simple-calendar/static/css/default.css")
	require("vue-simple-calendar/static/css/holidays-us.css")

	export default {
		data() {
			return { 
                showDate: new Date(),
                events : [],
                eventsInfo: [],
                innerVisible: false,
                outerVisible: false,
                eventClicked: new Event(),
                eventSaved: new Event(),
                eventChange: {}

            }
		},
		components: {
			CalendarView,
			CalendarViewHeader,
        },

        async created(){
            //setHours to compare date with endDate drag and drop
            this.showDate.setHours(0 ,0 ,0 ,0)
            this.eventsInfo = await GetEventsSchool()
            await this.CreateEvents()
        },
        
		methods: {
			setShowDate(d) {
				this.showDate = d;
            },
            
            async CreateEvents(){
                this.$root.$on('AddEvent', (event) =>{
                    this.AddEvent(event)
                })
                    
                    this.eventsInfo.forEach(element => {
                    this.AddEvent(element)
                })
            },

            onClickEvent(e) {
                this.outerVisible = true

                this.eventClicked.eventId = e.originalEvent.id
                this.eventClicked.begDate = e.originalEvent.startDate
                this.eventClicked.endDate = e.originalEvent.endDate
                this.eventClicked.name = e.originalEvent.title

                this.eventSaved = this.eventClicked.clone()
                this.eventChange = e.originalEvent
            },

            async changeDate() {
                 this.error = []

                if (!this.checkDate()) {
                    this.eventClicked = this.eventSaved
                    this.$message({showClose: true,duration: 5000, message: "Date de debut superieur ou egale à celle de fin",type: 'error'});
                } else {
                    try {
                    this.eventsInfo = await UpdateEvents(this.eventClicked)
                    this.$message({
                            message: "L'évènement " + this.eventClicked.name + " a bien été changé",
                            type: "success"
                    })
                    this.eventChange.startDate = this.eventClicked.begDate;
                    this.eventChange.endDate = this.eventClicked.endDate;
                    } catch (e) {
                        this.eventClicked = this.eventSaved
                        this.$message({
                            showClose: true,
                            duration: 5000,
                            message: e.message,
                            type: "error"
                        })
                    }

                }
            },

            AddEvent(event){
                var list = {}

                list.startDate = event.begDate
                list.endDate = event.endDate
                list.id = event.eventId
                list.title = event.name
                if(list.title.startsWith("ForumPI")){
                    console.log("ok")
                    list.classes = "orange"
                }
                if(list.title.startsWith("JPO")){
                    list.classes = "purple"
                }
                if(list.title.startsWith("Projet sur scene")){
                    list.classes = "blue"
                }
                this.events.push(list)
            },

           async DeleteEvent(){
                try {
                    await DeleteEvent(this.eventClicked.eventId)
                    
                    let idx = this.events.findIndex(x=> x.id === this.eventClicked.eventId)
                    this.events.splice(idx,1)
                    this.innerVisible = false
                    this.outerVisible = false

                } catch (e) {
                    this.$message({
                        showClose: true,
                        duration: 5000,
                        message: e.message,
                        type: "error"
                    });
                }
            },
            async onDrop(event, date) {
               
                const eLength = Math.floor((date.getTime() - event.startDate.getTime() ) / (1000*60*60*24)) + 1
         
                var begDate = new Date(event.startDate)
                var endDate = new Date(event.endDate)
                begDate.setDate(begDate.getDate() + eLength)
                endDate.setDate(endDate.getDate() + eLength)
                if(endDate < this.showDate){
                   this.$message({ message: "L'évènement " + event.title + " a une date de fin inférieur à celle d'aujourd'hui", type: "error"})
                }else{

                    let eventSend = new Event(event.originalEvent.id,  event.originalEvent.title,  begDate, endDate,false)
                    
                    try {
                        await UpdateEvents(eventSend)
                        this.$message({
                                message: "L'évènement " + eventSend.name + " a bien été changé",
                                type: "success"
                        })
                        event.originalEvent.startDate = begDate
                        event.originalEvent.endDate = endDate
                    } catch (e) {
                        this.$message({
                            showClose: true,
                            duration: 5000,
                            message: e.message,
                            type: "error"
                        })
                    }
                }
			},


            checkDate() {
                let begDate = new Date(this.eventClicked.begDate)
                let endDate = new Date(this.eventClicked.endDate)
                return begDate >= endDate ? false : true
            },

	}
}
</script>
<style>
.calendar-parent {
	display: flex;
	flex-direction: column;
	flex-grow: 1;
	overflow-x: hidden;
	overflow-y: hidden;
	max-height: 100vh;
	background-color: white;

	font-family: Calibri, sans-serif;
	width: auto;
    height: auto;
	min-width: 30rem;
	max-width: 100rem;
	min-height: 50rem;
   margin-left: 2%
}
 .maincal{
     height: 79.9vh;
 }




/*
**************************************************************
This theme is the default shipping theme, it includes some
decent defaults, but is separate from the calendar component
to make it easier for users to implement their own themes w/o
having to override as much.
**************************************************************
*/

/* Header */

.theme-default .cv-header,
.theme-default .cv-header-day {
	background-color: white;
}

.theme-default .cv-header .periodLabel {
	font-size: 1.5em;
}

.theme-default .cv-header button {
	color: black;
}

.theme-default .cv-header button:disabled {
	color: white;
	background-color: white;
}

/* Grid */

.theme-default .cv-day.past {
	background-color: white;
}

.theme-default .cv-day.outsideOfMonth {
	background-color: #b6b3b3;
}

.theme-default .cv-day.today {
	background-color: rgb(174, 221, 178);
}

/* Events */

.theme-default .cv-event {
	border-color: #e0e0f0;
	border-radius: 0.5em;
	background-color: #c2b05e;
	text-overflow: ellipsis;
    border-radius: 0%
}

.theme-default .cv-event.purple {
	background-color: #ca9bf7;
	border-color: #ca9bf7;
}

.theme-default .cv-event.orange {
	background-color: #a53d3d;
	border-color: #a53d3d;
}

.theme-default .cv-event.blue {
	background-color: #5272ad;
	border-color: #5272ad;
}

.theme-default .cv-event.continued::before,
.theme-default .cv-event.toBeContinued::after {
	content: " \21e2 ";
	color: #999;
}

.theme-default .cv-event.toBeContinued {
	border-right-style: none;
	border-top-right-radius: 0;
	border-bottom-right-radius: 0;
}

.theme-default .cv-event.isHovered.hasUrl {
	text-decoration: underline;
}

.theme-default .cv-event.continued {
	border-left-style: none;
	border-top-left-radius: 0;
	border-bottom-left-radius: 0;
}

/* Event Times */

.theme-default .cv-event .startTime,
.theme-default .cv-event .endTime {
	font-weight: bold;
	color: #666;
}

/* Drag and drop */

.theme-default .cv-day.draghover {
	box-shadow: inset 0 0 0.2em 0.2em yellow;
}
</style>
