import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AddUserComponent } from "./user/add-user/add-user.component";
import { UserListComponent } from "./user/user-list/user-list.component";
import { HeaderComponent} from "./header/header.component"

const routes: Routes = [
    { path: 'new-user', component: AddUserComponent},
    { path: 'user-list', component: UserListComponent},
    { path: ' ', component: HeaderComponent}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
export const routingComponents = [AddUserComponent,UserListComponent,HeaderComponent]