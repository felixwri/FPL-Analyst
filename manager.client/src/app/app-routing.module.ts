import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LeagueComponent } from './league/league.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'league',
    component: LeagueComponent
  },
  {
    path: 'league/:id',
    component: LeagueComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
