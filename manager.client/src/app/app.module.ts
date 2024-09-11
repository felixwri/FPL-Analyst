import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { LeagueComponent } from './league/league.component';
import { FormsModule } from '@angular/forms';
import { SearchIdComponent } from './components/search-id/search-id.component';
import { PersonalComponent } from './personal/personal.component';
import { FixturesComponent } from './personal/fixtures/fixtures.component';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { FooterComponent } from './footer/footer.component';
import { ChooseLeagueComponent } from './league/choose-league/choose-league.component';
import { StandingsComponent } from './league/standings/standings.component';
import { HistoryComponent } from './league/history/history.component';

@NgModule({
  declarations: [AppComponent, HomeComponent, LeagueComponent, PersonalComponent],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NavComponent,
    FormsModule,
    SearchIdComponent,
    FixturesComponent,
    FooterComponent,
    ChooseLeagueComponent,
    StandingsComponent,
    HistoryComponent,
  ],
  providers: [provideHttpClient(withInterceptorsFromDi()), provideCharts(withDefaultRegisterables())],
})
export class AppModule {}
