import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { AddMansionComponent } from './mansion/add-mansion/add-mansion.component';
import { DeleteMansionComponent } from './mansion/delete-mansion/delete-mansion.component';
import { EditMansionComponent } from './mansion/edit-mansion/edit-mansion.component';
import { MansionListComponent } from './mansion/mansion-list/mansion-list.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: '',
    children: [
      {
        path: 'auth',
        loadChildren: () =>
          import('./auth/auth.module').then((m) => m.AuthModule),
      },
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: 'mansion',
        component: MansionListComponent, 
        canActivate: [AuthGuard]
      },
      {
        path: 'mansion/add',
        component: AddMansionComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'mansion/edit/:id',
        component: EditMansionComponent
      },
      {
        path: 'mansion/delete/:id',
        component: DeleteMansionComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
