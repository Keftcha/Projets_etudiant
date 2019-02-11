// Vue router setup
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import requireAuth from './helpers/requireAuth';

// Components
import Home from './components/Home.vue'
import Login from './components/Login.vue'
import Logout from './components/Logout.vue'

//import Test from './components/Test.vue'
import About from './components/PIXI/About.vue'

import ViWork from './components/app/ViWork.vue'
import SchemaList from './components/app/components/SchemaList.vue'
import ShareGroup from './components/app/components/ShareGroup.vue'
import ShareUser from './components/app/components/ShareUser.vue'

import Alert from './components/Alert.vue'
import PixiTest from './components/PIXI/PixiTest.vue'

const routes = [
    { path: '', component: Home, },
    { path: '/home', component: Home, },
    { path: '/test', component: PixiTest},
    { path: '/about', component: About},

    { path: '/app/viwork', component: ViWork, beforeEnter: requireAuth,
        children: 
        [
            {
            path: 'schemalist',
            component: SchemaList
            },
            {
            path: 'sharegroup/:id',
            component: ShareGroup
            },
            {
            path: 'shareuser',
            component: ShareUser
            },
        ] 
    },

    {path: '/pixitest', component: PixiTest},

    { path: '/alert', component: Alert},
    
    { path: '/login', component: Login },
    { path: '/logout', component: Logout, beforeEnter: requireAuth },
];

export default new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});