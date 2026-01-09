# API Documentation

This document provides API specifications for developers consuming the Cricketrivia Backend API. It describes the available endpoints, their expected parameters, request bodies, and typical responses.

---

## Match Balls Mapping

### GET /api/MatchBallsMapping
Retrieves match ball mappings.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Match_id` (int)
  - `Team_id` (int)

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/MatchBallsMapping" -H "Content-Type: application/json" -d '{"Match_id": 0, "Team_id": 0}'
```

**Response notes:**
- Status Code: 200
- Body: Not specified

### PUT /api/MatchBallsMapping
Updates match ball mappings.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Id` (int, optional)
  - `BallNumber` (int, optional)
  - `Runs` (int, optional)
  - `Wicket` (bool, optional)

**Example curl command:**
```bash
curl -X PUT "https://api.example.com/api/MatchBallsMapping" -H "Content-Type: application/json" -d '{"Id": 0, "BallNumber": 0, "Runs": 0, "Wicket": false}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Id` (int, optional)
  - `BallNumber` (int, optional)
  - `Runs` (int, optional)
  - `Wicket` (bool, optional)

---

## Matches

### POST /api/Matches
Creates a new match.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Team1_id` (int, required)
  - `Team2_id` (int, required)
  - `NoOfBalls` (int, required)
  - `Team1_PlayersId` (List<int>, optional)
  - `Team2_PlayersId` (List<int>, optional)

**Example curl command:**
```bash
curl -X POST "https://api.example.com/api/Matches" -H "Content-Type: application/json" -d '{"Team1_id": 1, "Team2_id": 2, "NoOfBalls": 60, "Team1_PlayersId": [], "Team2_PlayersId": []}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `CreatedDate` (DateTime, optional)
  - `Team1Info` (TeamModel, optional)
  - `Team2Info` (TeamModel, optional)

### PUT /api/Matches
Posts match results.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Id` (int, optional)
  - `Team1_id` (int, optional)
  - `Team2_id` (int, optional)
  - `NoOfBalls` (int, optional)
  - `Team1_Total` (int, optional)
  - `Team1_Wickets` (int, optional)
  - `Team2_Total` (int, optional)
  - `Team2_Wickets` (int, optional)

**Example curl command:**
```bash
curl -X PUT "https://api.example.com/api/Matches" -H "Content-Type: application/json" -d '{"Id": 0, "Team1_id": 0, "Team2_id": 0, "NoOfBalls": 0, "Team1_Total": 0, "Team1_Wickets": 0, "Team2_Total": 0, "Team2_Wickets": 0}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Id` (int, optional)
  - `Team1_id` (int, optional)
  - `Team2_id` (int, optional)
  - `NoOfBalls` (int, optional)
  - `Team1_Total` (int, optional)
  - `Team1_Wickets` (int, optional)
  - `Team2_Total` (int, optional)
  - `Team2_Wickets` (int, optional)

### GET /api/Matches/{id}
Retrieves a match by its ID.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `id` (int)

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/Matches/{id}" -H "Content-Type: application/json" -d '{"id": 123}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `CreatedDate` (DateTime, optional)
  - `Team1Info` (TeamModel, optional)
  - `Team2Info` (TeamModel, optional)

---

## Players

### GET /api/Players
Retrieves all players.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body: Not specified

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/Players"
```

**Response notes:**
- Status Code: 200
- Body: Not specified

### POST /api/Players
Creates a new player.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Name` (string, optional)

**Example curl command:**
```bash
curl -X POST "https://api.example.com/api/Players" -H "Content-Type: application/json" -d '{"Name": "New Player"}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Id` (int, optional)
  - `Name` (string, optional)

### PUT /api/Players
Updates an existing player.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Id` (int, optional)
  - `Name` (string, optional)

**Example curl command:**
```bash
curl -X PUT "https://api.example.com/api/Players" -H "Content-Type: application/json" -d '{"Id": 1, "Name": "Updated Player Name"}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Id` (int, optional)
  - `Name` (string, optional)

### DELETE /api/Players/{id}
Deletes a player by ID.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `id` (int)

**Example curl command:**
```bash
curl -X DELETE "https://api.example.com/api/Players/{id}" -H "Content-Type: application/json" -d '{"id": 123}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Id` (int, optional)
  - `Name` (string, optional)

### GET /api/Players/{id}
Retrieves a player by ID.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `id` (int)

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/Players/{id}" -H "Content-Type: application/json" -d '{"id": 123}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Team` (TeamModel?, optional)

### GET /api/Players/NotMapped
Retrieves all players not currently assigned to a team.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body: Not specified

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/Players/NotMapped"
```

**Response notes:**
- Status Code: 200
- Body: Not specified

---

## Teams

### GET /api/Teams
Retrieves all teams.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body: Not specified

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/Teams"
```

**Response notes:**
- Status Code: 200
- Body: Not specified

### POST /api/Teams
Creates a new team.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `Name` (string, optional)
  - `PlayersId` (List<int>?, optional)

**Example curl command:**
```bash
curl -X POST "https://api.example.com/api/Teams" -H "Content-Type: application/json" -d '{"Name": "New Team", "PlayersId": []}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Players` (IEnumerable<PlayerModel>?, optional)

### PUT /api/Teams
Updates an existing team.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `PlayersId` (List<int>?, optional)

**Example curl command:**
```bash
curl -X PUT "https://api.example.com/api/Teams" -H "Content-Type: application/json" -d '{"PlayersId": [1, 2, 3]}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Players` (IEnumerable<PlayerModel>?, optional)

### DELETE /api/Teams/{id}
Deletes a team by ID.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `id` (int)

**Example curl command:**
```bash
curl -X DELETE "https://api.example.com/api/Teams/{id}" -H "Content-Type: application/json" -d '{"id": 123}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Id` (int, optional)
  - `Name` (string, optional)

### GET /api/Teams/{id}
Retrieves a team by ID.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body:
  - `id` (int)

**Example curl command:**
```bash
curl -X GET "https://api.example.com/api/Teams/{id}" -H "Content-Type: application/json" -d '{"id": 123}'
```

**Response notes:**
- Status Code: 200
- Body:
  - `Players` (IEnumerable<PlayerModel>?, optional)

---

## Weather Forecast

### GET /WeatherForecast/Name = "GetWeatherForecast"
Retrieves weather forecasts.

**Parameters:**
- Route parameters: Not specified
- Query parameters: Not specified
- Request body: Not specified

**Example curl command:**
```bash
curl -X GET "https://api.example.com/WeatherForecast/Name = \"GetWeatherForecast\""
```

**Response notes:**
- Status Code: 200
- Body: Not specified