import Vue from 'vue';
import Router from 'vue-router';

import requireAuth from './helpers/requireAuth';


import Home from './components/Home.vue';
import Login from './components/Login.vue';
import Logout from './components/Logout.vue';

import Test from './components/Test.vue';
import About from './components/About.vue';

import ViWork from './components/app/ViWork.vue';
import SchemaList from './components/app/components/SchemaList.vue';
import ShareGroup from './components/app/components/ShareGroup.vue';
import ShareUser from './components/app/components/ShareUser.vue';

import Alert from './components/Alert.vue';
import PixiTest from './components/PIXI/PixiTest.vue';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
    },
    {
      path: '/about',
      name: 'about',
      component: About,
    },
    {
      path: '/test',
      name: 'test',
      component: Test,
    },

    { path: '/app/viwork', component: ViWork, beforeEnter: requireAuth,
        children: [
            {
            path: 'schemalist',
            component: SchemaList,
            },
            {
            path: 'sharegroup/:id',
            component: ShareGroup,
            },
            {
            path: 'shareuser',
            component: ShareUser,
            },
        ],
    },

    { path: '/alert', component: Alert},
    { path: '/login', component: Login },
    { path: '/pixiTest', component: PixiTest},
    { path: '/logout', component: Logout, beforeEnter: requireAuth },

  ],
});
