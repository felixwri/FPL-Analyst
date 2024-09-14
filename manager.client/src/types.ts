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
