export interface TeamData {
  id: number;
  name: string;
  managerName: string;
  points: number;
  dateJoined: Date;
  leagues: any[];
}

export interface UpcomingFixtures {
  Id: number;
  Team: string;
  TeamHomeDifficulty: number;
  TeamAwayDifficulty: number;
  Fixtures: [
    {
      Id: number;
      AtHome: boolean;
      Opponent: string;
      RelativeDifficulty: number;
      OpponentDifficulty: number;
      Kickoff: Date;
    },
  ];
}

export interface leagueHistory {
  leagueId: number;
  leagueName: string;
  gameWeeks: [
    {
      gameWeek: number;
      scores: [
        {
          points: number;
          pointsOnBench: number;
        },
      ];
    },
  ];
}
