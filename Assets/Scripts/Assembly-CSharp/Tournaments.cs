using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tournaments : MonoBehaviour
{
	public static int numChampionships;

	public static int numTrophies;

	private List<Tournament> tournaments;

	public virtual void Awake()
	{
		InstantiateTournaments();
	}

	public virtual List<Tournament> GetTournaments()
	{
		return tournaments;
	}

	public virtual Tournament GetTournament(int num)
	{
		return tournaments[num];
	}

	public static int GetCurrentTournamentNum()
	{
		return PlayerPrefs.GetInt("TOURNAMENT_CURRENT_NUM");
	}

	public static void SetCurrentTournamentNum(int num)
	{
		PlayerPrefsHelper.SetInt("TOURNAMENT_CURRENT_NUM", num, true);
	}

	public static int GetCurrentRound(int num)
	{
		return PlayerPrefs.GetInt(Tournament.TOURNAMENT_CURRENT_ROUND_KEY + num);
	}

	public static bool IsReigningChamp(int num)
	{
		return PlayerPrefs.GetInt(Tournament.TOURNAMENT_REIGNING_CHAMP + num) == 1;
	}

	public static bool TournamentIsCompleted(int num)
	{
		return GetNumCompletions(num) >= 1;
	}

	public static int GetNumCompletions(int num)
	{
		return PlayerPrefs.GetInt(Tournament.TOURNAMENT_COMPLETED_KEY + num);
	}

	public virtual void InstantiateTournaments()
	{
		Debug.Log("--------Tournaments.InstantiateTournaments()----------------------");
		numChampionships = 0;
		numTrophies = 0;
		tournaments = new List<Tournament>();
		AddTournament(false, "MINNESOTA", -1, 800, 0, tournamentTypeEnum.ThreeRound, "ALLEY-OOPERS", "TEAM MAMBA.", "3BALLERZ", "POSTERIZERS!", "BBALL_KINGS", "THE VIPERS", "DUNKMASTERZ", ArenaChooser.GetMinnesotaArena());
		AddTournament(false, "WISCONSIN", 0, 900, 0, tournamentTypeEnum.ThreeRound, "SPIN SPYDERS", "LOS JAMERS", "BUZZER BEATERS", "THESNIPERS", "PURE SWISH", "FIREBALLS", "GR8-SHOOTERZ", 10);
		AddTournament(false, "MICHIGAN", 1, 1100, 0, tournamentTypeEnum.ThreeRound, "TEAM BEARS", "#BBALL.", "BIGDUNK2020", "REDROACHES", "SWISH BROS^^", "CA KNIGHTS", "DUNKERS.UNITED", 33);
		AddTournament(false, "ILLINOIS", 1, 1200, 0, tournamentTypeEnum.ThreeRound, "POSTERIZERS", "BANK SHOT!", "PYTHONS.", "TECH FOWLERS", "DA DUNKAZ", "HOGS 2018", "SNIPERKINGS", 68);
		AddTournament(true, "CHARLOTTE", 0, 1600, 0, tournamentTypeEnum.ThreeRound, "U GON LOOSE", "THE EUROS", "COYOTES", "LADY LUCK...", "ROYAL*RAIDERS", "TEAM X-OVER", "SPINNERS", 12);
		AddTournament(true, "INDIANA", 4, 1700, 0, tournamentTypeEnum.ThreeRound, "BBOARD.SMASHER", "ALL^STARS!", "BLUECRABS", "CROCODILES", "DA+DINGOS", "THE KODIAKS**", "TEAM+WOLVES", 26);
		AddTournament(false, "BOSTON", 3, 1900, 0, tournamentTypeEnum.ThreeRound, "HAH CRICKETS", "$PLASHIN", "SPLASH BROS", "FISHERS", "WOMBATS", "NET SHARKS", "PITBULLS", 65);
		AddTournament(false, "CINCINNATI", 5, 1200, 0, tournamentTypeEnum.ThreeRound, "ANKLEBREAKERS", "HOWLERS", "SHARKBAIT", "PIRANHAZ", "REVERSE^^***", "LOS BOOMERS", "THE BACKSPIN", 32);
		AddTournament(false, "PITTSBURGH", 6, 1100, 0, tournamentTypeEnum.ThreeRound, "HOOP BREAKERS", "PARROTS", "BALLNBLASTERS", "THE CHILLS", "BIGDOGS", "EZ BUCKETZ", "SOULS", 66);
		AddTournament(false, "NYC", 6, 2000, 0, tournamentTypeEnum.ThreeRound, "GREATS.", "JELLYFISH", "COURT STORMERS", "PIRATES.", "BACKBOARD SMASH", "BASKET BOSSES", "KINGFISHERS", 46);
		AddTournament(true, "WEST VIRGINIA", 9, 1500, 0, tournamentTypeEnum.ThreeRound, "THREE & D", "SAILS", "SNAPPERS", "HEROES", "BOAS", "CRUSHERS", "SHADOW95", 14);
		AddTournament(false, "PHILADELPHIA", 9, 1200, 0, tournamentTypeEnum.ThreeRound, "TEAM PROCESS", "BLOCKERS", "BIG BALLERS", "LA SHADOWERS", "SLIDERS", "SUPPLIERS", "DETAILERS", 64);
		AddTournament(true, "BALTIMORE", 11, 1900, 0, tournamentTypeEnum.ThreeRound, "GORILLAS", "TEAM TRIPLE-DOUBLE", "LYNXES", "RETRIEVERS", "BLOCKERS", "THE 36ERS", "SIDEWINDERS", 62);
		AddTournament(false, "KENTUCKY", 4, 800, 0, tournamentTypeEnum.ThreeRound, "PIGEONS", "SNAILS", "PEPPERS99", "HALL OF FAME", "ORCAS", "GNOMES", "GREMLINS", 39);
		AddTournament(true, "VIRGINIA", 10, 900, 0, tournamentTypeEnum.ThreeRound, "HARES", "COBRAS.", "BEETLES", "33ERS", "DINGOS", "OCTOPI", "SUPERSPEED", 28);
		AddTournament(false, "ATLANTIC COAST", 8, 1200, 0, tournamentTypeEnum.ThreeRound, "FALCONS", "BLUECRABS", "BALLERS", "SWIFTS", "PITBULLS", "59ERS", "TAILS", 2);
		AddTournament(true, "TENNESSEE", 12, 1200, 0, tournamentTypeEnum.ThreeRound, "99ERS", "ROACHES42", "REDS", "PREDATORS", "COOKIE JAR", "HEROES", "RATTLERS", 7);
		AddTournament(false, "NORTH CAROLINA", 16, 1200, 0, tournamentTypeEnum.ThreeRound, "B.A.T.S.", "REAL_ONES", "DRAGONS", "OWLS", "SQUIDS", "TOUCANS98", "ANTELOPES.", 29);
		AddTournament(true, "ATLANTA", 17, 1500, 0, tournamentTypeEnum.ThreeRound, "ROMANS", "BOOMERS", "HEROS_4", "BLUECRABS", "ROACHES", "HYDRAS", "CHAMPS", 1);
		AddTournament(false, "CHARLESTON", 17, 1100, 0, tournamentTypeEnum.ThreeRound, "PIPERS", "GREATS", "FINCHES", "WOMBATS", "CHAMELEONS", "RUMBLERS", "43ERS", 3);
		AddTournament(false, "JACKSONVILLE", 16, 1200, 0, tournamentTypeEnum.ThreeRound, "LORIKEETS", "SOULS", "GREAT GECKOS", "PIRATES", "PHEASANTS", "DARTS", "SHARKS", 8);
		AddTournament(true, "NEW ORLEANS", 16, 1400, 0, tournamentTypeEnum.ThreeRound, "THE VIPERS^", "PARROTS", "BLASTERS", "DIVERS 3000", "BIGDOGS", "TWEETERS", "TARANTULAS", 16);
		AddTournament(false, "MIAMI", 10, 1500, 0, tournamentTypeEnum.ThreeRound, "SPIDERS", "PEPPERS", "REPTILEZ", "TEAM OWLS", "LIONS", "DODGERS", "STORMERS", 67);
		AddTournament(true, "ALABAMA", 22, 800, 0, tournamentTypeEnum.ThreeRound, "TERRIERS", "AKITAS", "COYOTES", "COBRAS", "RAIDERS^", "DINOBIRDS", "COOL CRICKETS", 63);
		AddTournament(false, "MISSISSIPPI", 20, 900, 0, tournamentTypeEnum.ThreeRound, "DESSERTERS", "DISPLACERS", "TRAINERS", "HUSSELERS", "ELEMENTS", "BOUNCERS", "EAGLES", 20);
		AddTournament(false, "TALLAHASSEE", 20, 800, 0, tournamentTypeEnum.ThreeRound, "DINGOS", "RUMBLERS", "CHEETAHS", "SNIPERS", "NEUTRONS", "WOLVES", "RATTLERS", 54);
		AddTournament(true, "ARKANSAS", 21, 800, 0, tournamentTypeEnum.ThreeRound, "BATS", "JELLYFISH", "DRAGONS^^", "NET OWLS", "SQUIDS", "TOUCANS", "ANTELOPES", 5);
		AddTournament(false, "KANSAS CITY", 26, 1200, 0, tournamentTypeEnum.ThreeRound, "HERRINGS", "SAILS", "SNAPPERS", "HEROES", "BOAS", "CRUSHERS", "GATORS", 31);
		AddTournament(false, "HOUSTON", 26, 1600, 0, tournamentTypeEnum.ThreeRound, "SPIDERS", "HOT PEPPERS", "REPTILES", "OWLS", "LIONS", "DODGERS!", "NET STORMERS", 36);
		AddTournament(true, "DALLAS", 27, 1200, 0, tournamentTypeEnum.ThreeRound, "GORILLAS", "SETTERS", "LYNXES", "RETRIEVERS", "BLOCKERS", "36ERS", "SIDEWINDERS", 45);
		AddTournament(false, "SAN ANTONIO", 27, 1700, 0, tournamentTypeEnum.ThreeRound, "TERRIERZ!!", "AKITAS", "COYOTES", "GATORS%", "RAIDERS", "DINOBIRDS", "HUNTERS", 47);
		AddTournament(true, "OKLAHOMA CITY", 28, 1600, 0, tournamentTypeEnum.ThreeRound, "GIRAFFS", "KANGAROOS", "BLUECRABS", "CROCODILES", "DINGOS", "KODIAKS", "WOLVES", 49);
		AddTournament(false, "MISSOURI", 28, 1200, 0, tournamentTypeEnum.ThreeRound, "PIGEONS", "SNAILS", "PEPPERS", "ROMANS", "ORCAS", "GNOMES!!", "GREMLINS", 6);
		AddTournament(true, "NEBRASKA", 29, 1400, 0, tournamentTypeEnum.ThreeRound, "RUMBLERS", "RAIDERS", "ZEBRAS", "TIGERS", "FIREBALLZ", "VIPERS", "DOLPHINS", 11);
		AddTournament(false, "IOWA", 29, 900, 0, tournamentTypeEnum.ThreeRound, "DRAGONS", "SAILS", "SNAPPERS", "HEROES", "BOAS99", "COBRAS", "GATORS", 15);
		AddTournament(false, "LOS ANGELES", 21, 1700, 0, tournamentTypeEnum.ThreeRound, "BIGDOGS", "PARROTS", "BLASTERS", "NO DICE", "BREAKERS", "TWEETERS", "SOULS", 34);
		AddTournament(true, "LUBBOCK", 34, 750, 0, tournamentTypeEnum.ThreeRound, "GENERATORS", "HEALERS", "RISKERS", "TIGERS", "GIANTS", "BRAVERS", "CHAMPIONS", 35);
		AddTournament(false, "CHICAGO", 25, 1400, 0, tournamentTypeEnum.ThreeRound, "DA SPIDERS", "PEPPERS", "REPTILES", "OWLS", "LIONS", "DODGERS", "STORMERS", 37);
		AddTournament(true, "NORTH DAKOTA", 37, 700, 0, tournamentTypeEnum.ThreeRound, "FOLLOWERS", "PATRIOTS99", "TRUSTERS", "WESTERNERS", "DREAMERS", "VIPERS", "DOLPHINS", 52);
		AddTournament(false, "CLEVELAND", 30, 1400, 0, tournamentTypeEnum.ThreeRound, "GIRAFFS", "WOLVES", "BLUECRABS", "CROCODILES", "DINGOS", "KODIAKS", "KANGAROOS.", 42);
		AddTournament(false, "OAKLAND", 32, 1500, 0, tournamentTypeEnum.ThreeRound, "PANTHERS", "MYSTERIES", "ZEBRAS", "TIGERS", "FIREBALLS", "AMAZING BALLER", "DOLPHINS", 38);
		AddTournament(true, "NORTH CAL", 36, 450, 0, tournamentTypeEnum.ThreeRound, "LORIKEETS", "SOULS", "GECKOS", "PIRATES", "PHEASANTS99", "DARTSX", "SHARKS", 17);
		AddTournament(false, "PORTLAND", 37, 1100, 0, tournamentTypeEnum.ThreeRound, "BOOMERS.", "HOWLERS", "GOLDFISH", "LOOMERS", "SHEEPDOGS", "PEACOCKS", "PIRANHAS", 29);
		AddTournament(true, "BOISE", 38, 1400, 0, tournamentTypeEnum.ThreeRound, "LOCKERS88", "RAPTORS", "DRAGONS", "TARANTULAS", "HOUNDS", "FLAMINGOS", "ORCAS", 40);
		AddTournament(true, "MONTANA", 39, 650, 0, tournamentTypeEnum.ThreeRound, "BATS", "JELLYFISH", "DRAGONS", "OWLS", "SQUIDS", "TOUCANS88", "ANTELOPES", 30);
		AddTournament(false, "SALT LAKE CITY", 32, 1100, 0, tournamentTypeEnum.ThreeRound, "RUMBLERSX", "TEAM WOW", "ZEBRAS", "TIGERS", "MUSICIANS", "VIPERS", "DOLPHINS", 53);
		AddTournament(true, "KANSAS", 36, 1600, 0, tournamentTypeEnum.ThreeRound, "RACOONS", "CREEPS77", "CROWS", "SQUIDS", "REPTILES", "ELEPHANTS", "FROGS", 19);
		AddTournament(false, "PHOENIX", 38, 1400, 0, tournamentTypeEnum.ThreeRound, "MR DIAMONDS", "MINERS", "UPGRADERS", "WATCHERS", "SHEPHARDS 7", "DUNKERS34", "LIONS", 43);
		AddTournament(true, "DENVER", 37, 1200, 0, tournamentTypeEnum.ThreeRound, "PAPPAS", "MAMMAS 34", "TALKERS", "PLANNERS", "BLAMERS", "DRAFT-TANKERS", "FIGHTERS", 40);
		AddTournament(false, "LAS VEGAS", 39, 1200, 0, tournamentTypeEnum.ThreeRound, "BOXERS 2", "REALERS", "TREATERS", "PAYERS", "CROSSERS", "MAPPERS", "SKIIERS", 4);
		AddTournament(false, "SEATTLE", 30, 1500, 0, tournamentTypeEnum.ThreeRound, "JETS", "LEAFS", "OILERS", "CUBS", "HIPSTERS", "LAUNCHERS", "LIZARDS", 55);
		AddTournament(true, "OREGON", 41, 1200, 0, tournamentTypeEnum.ThreeRound, "PENGUINS", "FALCONS", "OUTLAWS", "LUMBERJACKS", "SHAMROCKS", "BLACKWOLVES", "SHARPSHOOTERS", 44);
		AddTournament(true, "RENO", 42, 1400, 0, tournamentTypeEnum.ThreeRound, "BLACKHAWKS", "SABRES", "THE RAVENS", "SENATORS", "SHOOTINGSTARS", "NINJAS", "TARGETERS", 6);
		AddTournament(false, "NEVADA", 43, 1250, 0, tournamentTypeEnum.ThreeRound, "RATTLERS", "RANGERS", "THE BRAVES", "BILLS", "PEASHOOTERS 34", "CHAOS", "GRENADES", 19);
		AddTournament(false, "SACRAMENTO", 44, 1400, 0, tournamentTypeEnum.ThreeRound, "WINGS 99", "MONSTERS", "SWARMERS", "WILDCATS", "TEXANS", "SPARTONS", "BEAVERS", 41);
		AddTournament(true, "DEATH VALLEY", 35, 2400, 0, tournamentTypeEnum.ThreeRound, "THE SNAKES", "VALLEYCATS", "SPIDERS", "DIAMONDBACKS", "BISON", "WARRIORS", "COUGARS", 9);
		AddTournament(false, "SAN DIEGO", 36, 1400, 0, tournamentTypeEnum.ThreeRound, "FLAMES", "ROYALS", "LOGNUTS", "TEXANS", "SQUIRRELS", "BORDERERS", "OCEANBATS", 8);
		AddTournament(true, "CHANNEL ISLANDS", 47, 800, 0, tournamentTypeEnum.ThreeRound, "SWIMMERS", "CHANNELERS", "DUCKS", "DIPPERS", "FISHERS", "WHALES", "SURFERS", 3);
		AddTournament(false, "CATALINA ISLAND", 48, 900, 0, tournamentTypeEnum.ThreeRound, "ISLANDERS", "SWIMMERS2000", "SHOREBIRDS", "DA RAYS", "REEFS", "ASTROS", "RIPTIDE", 22);
		AddTournament(true, "SAN NICOLAS ISLAND", 49, 1000, 0, tournamentTypeEnum.ThreeRound, "DODGERS", "BREWERS", "SHARKS", "BUCS", "MARINERS", "CRABBERS", "DEEPSEAERS", 23);
		AddTournament(false, "TUSCAN", 50, 1100, 0, tournamentTypeEnum.ThreeRound, "PIRATES", "TIGERS", "JAGUARS!", "GREEDYGOATS", "TEAMREBELS", "CAMELS", "BANDITS", 24);
		AddTournament(true, "ALBUQUERQUE", 51, 1200, 0, tournamentTypeEnum.ThreeRound, "MARLINS", "DOLPHINS", "TITANS", "BISCUITS", "HOOPIN HENS", "BEARCATS", "HORNETS", 18);
		AddTournament(false, "ARIZONA", 52, 1300, 0, tournamentTypeEnum.ThreeRound, "LIGHTNING", "PRESIDENTS", "PIONEERS", "RANGERS", "DASHERS!!!", "FROGS", "RAVENS", 48);
		AddTournament(false, "NEW MEXICO", 53, 1400, 0, tournamentTypeEnum.ThreeRound, "CHIHUAHUAS", "REBELS", "STEALTHERS", "BLAZERS", "SNAPPERS", "PIRATES", "DODGERS", 1);
		AddTournament(true, "GRAND CANYON", 38, 1300, 0, tournamentTypeEnum.ThreeRound, "AVALANCH", "BIG ROCKS", "DEEPERS", "ROCKERS", "RIVERSNAKES", "SALON", "WILDERS", 50);
		AddTournament(true, "FISH LAKE", 55, 1200, 0, tournamentTypeEnum.ThreeRound, "BAYHAWKS", "SALMONBELLIES", "SEADOGS!!!", "SEACATS", "TROUT2020", "LAKECATS", "LAKEBEARS", 2);
		AddTournament(false, "BATTLE MOUNTAIN", 32, 1100, 0, tournamentTypeEnum.ThreeRound, "ROUGHNECKS", "ATTACKERS", "RUSHERS", "BATTLERS", "FIGHERS", "WINNERS", "TUFFGUYS", 5);
		AddTournament(false, "SALTON SEA", 56, 1000, 0, tournamentTypeEnum.ThreeRound, "SEACATS", "SEAFISH", "SEAWOLVES", "SEATROUT", "SEASNAKES", "SEABEARS", "SEAROCKS", 11);
		AddTournament(true, "EL PASO", 57, 900, 0, tournamentTypeEnum.ThreeRound, "EL DIABLOS", "EL SNIPERS", "EL GOATS", "PADRES", "BANDITS", "TEXANS", "SNAKES", 13);
		AddTournament(true, "SILVER CITY", 55, 800, 0, tournamentTypeEnum.ThreeRound, "SILVERCLUBS", "JAGUARS", "SILVERBACKS", "SILVERSWARMERS", "SILVERSHOOTERS", "SILVERBOAS", "SILVERDOGS", 18);
		AddTournament(true, "FORT TITANS", 58, 800, 0, tournamentTypeEnum.ThreeRound, "TITANS", "BAYHAWKS", "GRENADES", "CHIHUAHUAS", "PIRATES", "STEALTHERS", "REBELS", 21);
		AddTournament(false, "BATTLE CREEK", 50, 900, 0, tournamentTypeEnum.ThreeRound, "BATTLEDOGS", "BATTLECATS", "BATTLEPIGS", "BATTLEBATS", "BATTLECARS", "TANKS", "BATTLEKIDS", 63);
		AddTournament(true, "TRAVERSE CITY", 60, 1000, 0, tournamentTypeEnum.ThreeRound, "BLAZERS", "SABRES", "RANGERS", "LAUNCHERS", "HENS", "SPARTONS", "OWLS", 51);
		AddTournament(false, "BUFFALO", 62, 1100, 0, tournamentTypeEnum.ThreeRound, "OUTLAWS", "CANADIANS", "THE FALCONS", "CHIHUAHUAS", "PITBULLS", "HEROES", "BUFFALOS", 62);
		AddTournament(true, "SYRACUSE", 63, 1200, 0, tournamentTypeEnum.ThreeRound, "DRAGONS", "FLAMES", "PRESIDENTS", "ROCKERS", "RUNNERS", "HENS", "RANGERS", 69);
		AddTournament(true, "MAINE", 64, 1400, 0, tournamentTypeEnum.ThreeRound, "LOONS", "SABRES", "RUSHERS", "CROWS", "ELK", "BADCATS", "MAINSHOWS", 50);
		AddTournament(false, "VERMONT", 60, 1500, 0, tournamentTypeEnum.ThreeRound, "NINJAS", "COWBOYS", "LAUNCHERS", "BALLERS", "SPARTONS", "DA BILLS", "SKIIERS", 72);
		AddTournament(true, "BERMUDA", 65, 1400, 0, tournamentTypeEnum.ThreeRound, "SWIMMERS", "TOURISTS", "JETHAWKS", "BOATERS", "OCEANFROGS", "RUSHERS", "SEASNIPERS", 67);
		AddTournament(false, "ORLANDO", 66, 1300, 0, tournamentTypeEnum.ThreeRound, "SWARMERS", "LARGE BILLS", "BALLERS1985", "BADDOGS", "BISCUITS", "LAUNCHERS", "RANGERS", 28);
		AddTournament(true, "DAYTONA BEACH", 60, 1400, 0, tournamentTypeEnum.ThreeRound, "PARTYGOERS", "RACERS", "SUNBATHERS", "BEACHPIGS", "BEACHTIGERS", "SUNRAYS", "SWIMMERS", 34);
		AddTournament(true, "TAMPA", 68, 1400, 0, tournamentTypeEnum.ThreeRound, "CHIHUAHUAS", "SPARTONS", "COBRAS", "CROWS", "WINDMILLERS", "HENS", "CRABBERS", 35);
		AddTournament(false, "FREEPORT", 69, 1100, 0, tournamentTypeEnum.ThreeRound, "RUSHERS", "JAGUARS", "OUTLAWS", "BISCUITS", "BALLERS", "JETTERS", "ROCKERS", 36);
		AddTournament(true, "SANDY POINT", 70, 1400, 0, tournamentTypeEnum.ThreeRound, "SANDFROGS", "CRABBERS", "POINTERS", "SANDCARS", "SANDSNAKES", "SANDRATS", "FALCONS", 37);
		AddTournament(false, "CONECUH FOREST", 71, 1200, 0, tournamentTypeEnum.ThreeRound, "RAYS", "HENS", "GATORS", "COWBOYS", "SPARTONS", "PYTHONS", "LAUNCHERS", 74);
		AddTournament(true, "DE SOTA FOREST", 72, 1250, 0, tournamentTypeEnum.ThreeRound, "ROCKERS", "RACERS", "RANGERS", "BILLS", "NINJAS", "BISCUITS", "SWARMERS", 75);
		AddTournament(false, "YELLOW STONE", 73, 1400, 0, tournamentTypeEnum.ThreeRound, "EXPLORERS", "HIKERS", "GEYSERS", "FAITHFULLS", "CAMPERS", "WOLVES", "GRIZZLIES", 76);
		AddTournament(true, "GLACIAR NAT PARK", 50, 3100, 0, tournamentTypeEnum.ThreeRound, "CAMPERS", "TRAVELERS", "TOURISTS", "GLACIERS", "ICEBURGHS", "TRAILERS", "CLIMBERS", 77);
		AddTournament(false, "ARCHES NAT PARK", 75, 1100, 0, tournamentTypeEnum.ThreeRound, "BLACKBEARS", "WOLVES", "WILDBOARS", "EXPLORERS", "TOURISTS", "BIRDWATCHERS", "HIKERS", 1);
		AddTournament(false, "KINGSVILLE", 76, 1400, 0, tournamentTypeEnum.ThreeRound, "SABRES", "OUTLAWS", "RUSHERS", "COBRAS", "HORSEMEN", "KNIGHTS", "KINGS", 2);
		AddTournament(true, "LAREDO", 77, 1400, 0, tournamentTypeEnum.ThreeRound, "BILLS", "COWBOYS", "RAYS", "OWLS1999", "GATORS", "BATTERS", "HILLCATS", 3);
		AddTournament(false, "LAKE CHARLES", 78, 1400, 0, tournamentTypeEnum.ThreeRound, "NINJAS", "RANGERS", "WATERSNAKES", "JAGUARS", "BALLERS", "WATERNETS", "LAKERS", 4);
		AddTournament(true, "SHREVEPORT", 79, 1400, 0, tournamentTypeEnum.ThreeRound, "BISCUITS", "SUPERSNAKES", "SPARTONS", "PORTERS", "RACERS", "ELKERS", "FALCONS", 5);
		AddTournament(false, "PINEBLUFF", 80, 1200, 0, tournamentTypeEnum.ThreeRound, "OUTLAWS", "PINNERS", "ROCKERS", "HENS", "PYTHONS", "BLUFFERS", "LAUNCHERS", 6);
		AddTournament(true, "DEVILS LAKE", 81, 1200, 0, tournamentTypeEnum.ThreeRound, "DARKLAKERS", "SWARMERS", "DEMONS", "LAKEDOGS", "DEFILERS", "DARKBIRDS", "LAKESKULLS", 7);
		AddTournament(false, "LAKE SAKAKAWEA", 82, 1400, 0, tournamentTypeEnum.ThreeRound, "BALLERS", "THE GATORS", "HEROES", "WOLVES", "NINJAS", "TEAM SAKAS", "BILLS", 8);
		AddTournament(true, "FORT PECK", 83, 1100, 0, tournamentTypeEnum.ThreeRound, "SUPERGIANTS", "OUTLAWS", "RANGERS", "LAUNCHERS", "PYTHONS", "FORTERS", "RAYS", 9);
		AddTournament(false, "MILES CITY", 84, 1400, 0, tournamentTypeEnum.ThreeRound, "RACERS", "HILLCATS", "NINJAS", "FASTCARS", "RUSHERS", "SMARTJETS", "SABRES", 10);
		AddTournament(false, "THUNDER BASIN", 85, 1400, 0, tournamentTypeEnum.ThreeRound, "COBRAS", "DRAGGERS", "OUTLAWS", "DRAGONS", "HENS", "PITBULLS", "FALCONS", 11);
		AddTournament(true, "RAPID CITY", 86, 1200, 0, tournamentTypeEnum.ThreeRound, "COWBOYS", "EXPLORERS", "CROWS", "HILLCATS", "BALLERS", "PYTHONS", "RAPIDS", 12);
		AddTournament(false, "BIG HORN", 87, 1400, 0, tournamentTypeEnum.ThreeRound, "PATS", "SHEEP", "ROCKERS", "HORNS", "OUTLAWS", "WEAVERS", "RACERS", 13);
		AddTournament(true, "GRAND FORKS", 88, 1400, 0, tournamentTypeEnum.ThreeRound, "RAIDERS", "MOHAWKS", "SMASHERS", "FORKERS", "BLUECLAWS", "CROSSERS", "HEROES", 15);
		AddTournament(false, "LINCOLN", 89, 1100, 0, tournamentTypeEnum.ThreeRound, "SEAHAWKS", "RANGERS", "CHIHUAHUAS", "DRAGONS", "HOOP CROWS", "DOLLARS", "GATORS", 16);
		AddTournament(true, "VALENTINE", 90, 1100, 0, tournamentTypeEnum.ThreeRound, "EAGLES", "LAUNCHERS", "HENS", "COWS", "NINJAS", "SMARTERS", "VALENTINES", 17);
		AddTournament(false, "ALLIANCE", 91, 900, 0, tournamentTypeEnum.ThreeRound, "RUSHERS", "GIANTS", "PREDATORS", "STARS", "RACERS", "BILLS", "OUTLAWS", 18);
		AddTournament(true, "MCCOOK", 92, 1200, 0, tournamentTypeEnum.ThreeRound, "ROCKERS", "BRONCOS", "BLUECLAWS", "SPARTONS", "COOKS", "BALLERS", "COOKERS", 19);
		AddTournament(true, "WICHITA FALLS", 93, 1400, 0, tournamentTypeEnum.ThreeRound, "FALLERS", "VIKINGS", "RANGERS", "HURRICANES", "THRASHERS", "CROWS", "DRAGONS", 20);
		AddTournament(false, "ROCKY MOUNTAINS", 94, 1000, 0, tournamentTypeEnum.ThreeRound, "THRASHERS", "STEELERS", "OUTLAWS", "RANGERS", "MOUNTANIERS", "OWLS", "FALCONS", 21);
		AddTournament(true, "ATLANTIS", 55, 900, 0, tournamentTypeEnum.ThreeRound, "MERMAIDS", "STARFISH", "SEAWOLVES", "TREASURES", "OCEANKINGS", "ANCIENTS", "SEAQUEENS", 22);
		AddTournament(false, "CEDAR KEY", 96, 1400, 0, tournamentTypeEnum.ThreeRound, "KNIGHTS", "PACKERS", "PANTHERS", "HENS", "EXPLORERS", "DRAGONS", "CEDARWOLVES", 23);
		AddTournament(true, "DEL RIO", 97, 1100, 0, tournamentTypeEnum.ThreeRound, "HILLCATS", "SHIMPERS", "CARDINALS", "BILLS", "DRAGONS", "CUBS", "DEL RIOS", 24);
		AddTournament(false, "WALLA WALLA", 98, 1100, 0, tournamentTypeEnum.ThreeRound, "WALLAS", "BEARS", "BLUECLAWS", "RACERS", "LAUNCHERS", "SUPERSNAKES", "CROWS", 25);
		AddTournament(true, "WEST WENDOVER", 99, 1400, 0, tournamentTypeEnum.ThreeRound, "KNIGHTHAWKS", "SPARTONS", "RAMS", "LOOKOUTS", "CUBS", "WESTERS", "THRASHERS", 26);
		AddTournament(false, "WICHITA", 100, 1400, 0, tournamentTypeEnum.ThreeRound, "BILLS", "OUTLAWS", "RANGERS", "SAINTS", "NOTHIN BUT NET", "PYTHONS", "DRAGONS", 40);
		AddTournament(true, "GARDEN CITY", 101, 900, 0, tournamentTypeEnum.ThreeRound, "LEAFERS", "BIGTREES", "SABRES", "GUARDERS", "CHARGERS", "PLANTERS", "GARDENERS", 28);
		AddTournament(true, "TEXHOMA", 90, 1200, 0, tournamentTypeEnum.ThreeRound, "DRILLERS", "MISSIONS", "JETHAWKS", "HILLCATS", "CUBS", "LIONS", "THE FREEKS", 29);
		AddTournament(false, "AUGUSTA", 103, 1200, 0, tournamentTypeEnum.ThreeRound, "CHARGERS", "THRASHERS", "CHIPPERS", "DRIVERS", "RUSHERS", "PUTTERS", "GOLFERS", 30);
		AddTournament(true, "ST LOUIS", 104, 900, 0, tournamentTypeEnum.ThreeRound, "BALL HAWKS", "ARCHES", "FALCONS", "EXPLORERS", "PANTHERS", "DRAGONS", "SAINTS", 31);
		AddTournament(false, "DURANGO", 115, 1100, 0, tournamentTypeEnum.ThreeRound, "BILLS", "HENS", "DURANGOS", "CUBS", "SPARTONS", "ZEBRAS", "HEROES", 32);
		AddTournament(false, "BIG SPRING", 116, 1200, 0, tournamentTypeEnum.ThreeRound, "SPRINGERS", "JETHAWKS", "LAUNCHERS", "RANGERS", "DRAGONS", "SHOOTERS", "PROCESSORS", 33);
		AddTournament(true, "ELY", 117, 1400, 0, tournamentTypeEnum.ThreeRound, "SMOKERS", "RAYS", "CHARGERS", "RIVERSNAKES", "RACERS", "PYTHONS", "THRASHERS", 34);
		AddTournament(false, "BAKER CITY", 118, 800, 0, tournamentTypeEnum.ThreeRound, "BILLS", "RANGERS", "BLUECLAWS", "EXPLORERS", "ZEBRAS", "PANTHERS", "BAKERS", 35);
		AddTournament(true, "HART MOUNTAIN", 100, 1200, 0, tournamentTypeEnum.ThreeRound, "BLUECLAWS", "SPARTONS", "MOUNTAINDOGS", "CUBS", "PANTHERS", "EXPLORERS", "CHARGERS", 36);
		AddTournament(false, "AMERICAN FALLS", 120, 800, 0, tournamentTypeEnum.ThreeRound, "THRASHERS", "OUTLAWS", "AMERICANS", "RIM RAIDERS", "JETHAWKS", "RIVERSNAKES", "FALLERS", 37);
		AddTournament(true, "ROCK SPRINGS", 121, 900, 0, tournamentTypeEnum.ThreeRound, "ROCKDOGS", "ROCKBIRDS", "HENS", "ROCKCATS", "ROCKRANGERS", "BLACKROCKS", "WHITEROCKS", 38);
		AddTournament(false, "SPRINGER", 122, 1400, 0, tournamentTypeEnum.ThreeRound, "RAIDERS", "RIVERSNAKES", "CUBS", "SPARTONS", "RANGERS", "CHARGERS", "LAUNCHERS", 39);
		bool flag = Players.GetActiveStarterNum(true) > -1;
		AddTournament(PlayerPrefs.GetInt("LB_VERSION") % 2 == 0 && flag, ArenaChooser.GetLiveEventName(), 1, 0, 0, tournamentTypeEnum.LiveEvent, "THE SWISHERS", "ON FIRE!", "THE SUPERSTARZ", "ELEVATORS^", "DA DUNKERZ", "TEAM ELITE", "DINO THRASHERZ", ArenaChooser.GetLiveEventArena());
	}

	private void AddTournament(bool isFemale, string name, int prereq, int cash, int gold, tournamentTypeEnum type, string o1, string o2, string o3, string o4, string o5, string o6, string o7, int arena)
	{
		Tournament tournament = new Tournament();
		tournament.isFemale = isFemale;
		tournament.name = name;
		tournament.prereq = prereq;
		tournament.type = type;
		tournament.num = tournaments.Count;
		tournament.arena = arena;
		int xpPrize = (tournament.xpPrizeForFirstWin = ((tournament.num <= 1) ? 250 : 100));
		int numCompletions = GetNumCompletions(tournament.num);
		if (numCompletions > 0)
		{
			xpPrize = 85;
			gold = 0;
		}
		numChampionships += numCompletions;
		if (numCompletions >= 3 && type == tournamentTypeEnum.ThreeRound)
		{
			numTrophies++;
		}
		tournament.xpPrize = xpPrize;
		tournament.cashPrize = cash;
		tournament.goldPrize = gold;
		tournament.locked = false;
		tournament.completed = numCompletions > 0;
		tournament.currentRound = tournament.GetCurrentRound();
		tournament.opponentNames.Add(o1);
		tournament.opponentNames.Add(o2);
		tournament.opponentNames.Add(o3);
		tournament.opponentNames.Add(o4);
		tournament.opponentNames.Add(o5);
		tournament.opponentNames.Add(o6);
		tournament.opponentNames.Add(o7);
		tournaments.Add(tournament);
	}
}
