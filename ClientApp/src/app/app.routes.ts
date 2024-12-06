import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserListComponent } from './user-list/user-list.component';
import { authGuard } from './guards/auth.guard';
import { UserInfoComponent } from './user-info/user-info.component';
import { adminGuardGuard } from './guards/admin-guard.guard';

export const routes: Routes = [
    {path: '', redirectTo: '/login', pathMatch: 'full'},
    {path: 'register', component: UserRegistrationComponent},
    {path: 'login', component: LoginComponent},
    {
        path: 'dashboard',
        component: DashboardComponent, 
        children:[
        {
             path: 'info', 
             component: UserInfoComponent, 
        },
        {
            path: 'users', 
            component: UserListComponent, canActivate: [adminGuardGuard],
        }], 
        canActivate: [authGuard]
    },
    //{path: 'user-list', component: UserListComponent}
];
