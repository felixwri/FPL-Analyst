import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavComponent } from "./nav/nav.component";
import { LeagueComponent } from './league/league.component';
import { FormsModule } from '@angular/forms';
import { SearchIdComponent } from './components/search-id/search-id.component';
import { PersonalComponent } from './personal/personal.component';
import { FixturesComponent } from './personal/fixtures/fixtures.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LeagueComponent,
    PersonalComponent,
    FixturesComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NavComponent,
    FormsModule,
    SearchIdComponent,
],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
