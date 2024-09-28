export interface TeamData {
  id: number;
  name: string;
  managerName: string;
  points: number;
  dateJoined: Date;
  leagues: any[];
}

export interface UpcomingFixtures {
  id: number;
  team: string;
  teamHomeDifficulty: number;
  teamAwayDifficulty: number;
  fixtures: [
    {
      id: number;
      atHome: boolean;
      opponent: string;
      relativeDifficulty: number;
      opponentDifficulty: number;
      kickoff: Date;
    },
  ];
}

export interface LeagueData {
  id: number;
  name: string;
  size: number;
  week: number;
  teams: [
    {
      id: number;
      name: string;
      managerName: string;
      totalPoints: number;
      gameweekPoints: number;
      rank: number;
      lastRank: number;
    },
  ];
}

export interface League {
  leagueId: number;
  leagueName: string;
  history: LeagueHistory[];
}

export interface LeagueHistory {
  id: number;
  name: string;
  points: number[];
  points_on_bench: number[];
  total_points: number[];
}

export enum Positions {
  Goalkeeper = 'Goalkeeper',
  Defender = 'Defender',
  Midfielder = 'Midfielder',
  Forward = 'Forward',
}

export enum PositionsShort {
  GKP = 'Goalkeeper',
  DEF = 'Defender',
  MID = 'Midfielder',
  FWD = 'Forward',
}

export interface Player {
  isLive: boolean;
  multiplier: number;
  imageCode: number;
  benchOrder: number;
  id: number;
  firstName: string;
  secondName: string;
  teamId: number;
  teamName: string;
  position: Positions;
  positionShort: PositionsShort;
  weekPoints: number;
  totalPoints: number;
  bonusPoints: number;
  minutes: number;
  goalsScored: number;
  expectedGoalsScored: string;
  assists: number;
  cleanSheets: number;
  goalsConceded: number;
  ownGoals: number;
  saves: number;
}

export class Picks {
  public id: number = 0;
  public team: string = '';
  public managerName: string = '';
  public picks: Player[] = [];
}
