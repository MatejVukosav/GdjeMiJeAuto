﻿using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Collections.Generic;
using System.Globalization;

namespace Gdje_mi_je_auto1
{
	public static class GeoFencer 
	{
		private static bool isInitialized = false;

		private static PolygonOptions[] zones = new PolygonOptions[34];
		private const int red = unchecked((int) 0x60FF0000);
		private const int darkRed = unchecked((int) 0x808A0500);
		private const int yellow = unchecked((int) 0x60FFD700);
		private const int green = unchecked((int) 0x60ADFF2F);
		private const int cyan = unchecked((int) 0x6000FFFF);
		private const int blue = unchecked((int) 0x6000008B);
		private const int white = unchecked((int) 0x60FFFFFF);
		private static readonly int[] zoneColors = {cyan,cyan,cyan,red,green,green,green,green,green,yellow,green,green,green,green,green,yellow,yellow,yellow,yellow,yellow,yellow,yellow,yellow,yellow,green,blue,blue,blue,red,darkRed,green,yellow,darkRed,darkRed};

		private static void Initialize()
		{
			#region xlist
			String[] xList = {
				"45.7770793,45.7770793,45.77708678,45.77710923,45.77711671,45.77696706,45.77663781,45.77633101,45.7762038,45.77605414,45.77594938,45.77590448,45.77588951,45.77587455,45.77587455,45.77588203,45.77608407,45.77628611,45.77651808,45.77687726,45.7770344",
				"45.78335705,45.78340943,45.78342439,45.78347676,45.78353662,45.7835441,45.78328223,45.78299792,45.78247417,45.78212999,45.78177833,45.78129946,45.78100017,45.7803941,45.78011725,45.7798404,45.77945879,45.7792418,45.77911459,45.77908466,45.77910711,45.77910711,45.77909215,45.77907718,45.77907718,45.77908466,45.78016215,45.78123212,45.78236942",
				"45.7773188,45.77729635,45.77728887,45.77728139,45.77764804,45.77797728,45.77844121,45.7787031,45.77898744,45.77906227,45.7790922,45.77914458,45.77909968,45.77902486,45.77884527,45.77868814,45.77847114,45.77828408,45.77811946,45.77788749,45.77761811",
				"45.80899954,45.80994931,45.81110098,45.81216288,45.81321729,45.81326216,45.81323972,45.81316494,45.81308269,45.8128733,45.812634,45.81243957,45.8123947,45.81255922,45.81285087,45.81353884,45.81395761,45.81409969,45.81466052,45.81525127,45.81598408,45.81637291,45.81647012,45.81639535,45.81620093,45.81559524,45.81510171,45.81464557,45.81400247,45.81386039,45.81370336,45.8133145,45.81329955,45.81338181,45.81342667,45.81329955,45.81321729,45.81292565,45.81316494,45.81329955,45.81336685,45.81338928,45.81401743,45.81490729,45.81526622,45.81537839,45.81514658,45.81433898,45.81344911,45.81336685,45.81335189,45.81329955,45.81386787,45.81415951,45.81448106,45.81427168,45.81415951,45.81389031,45.81386039,45.81332946,45.81333694,45.81345658,45.81367345,45.81397256,45.81459322,45.81518397,45.81577471,45.81638039,45.81718796,45.81774129,45.81831705,45.81895262,45.81943116,45.81955827,45.81989474,45.8203957,45.82076208,45.82120322,45.82225745,45.82274344,45.82273596,45.82251166,45.82229484,45.82166678,45.82064992,45.81937882,45.81865353,45.81857875,45.81860866,45.8184666,45.8180703,45.81741229,45.81710571,45.81730013,45.81742724,45.81786093,45.8179058,45.81791327,45.81792823,45.81804787,45.81811516,45.81809273,45.81797309,45.81789084,45.81782354,45.81782354,45.8177712,45.81756184,45.81703094,45.81662715,45.81652247,45.81650003,45.81663463,45.81670193,45.81665706,45.81653742,45.81651499,45.81638039,45.81622336,45.81608129,45.81584948,45.81561767,45.81521388,45.81503441,45.81448853,45.81413708,45.81398752,45.81380805,45.81376318,45.81378561,45.813823,45.81388283,45.81391274,45.81394265,45.81398004,45.8143988,45.81489981,45.81529613,45.81549803,45.81595417,45.81652247,45.81696364,45.81632805,45.81567002,45.8152064,45.81483999,45.81406977,45.81340424,45.8130453,45.81255174,45.81230497,45.81193106,45.81163941,45.81137019,45.81085419,45.81017366,45.80959034,45.80917903,45.80884249,45.80807967,45.80762347,45.80629971,45.80606039,45.80539476,45.80509559,45.80500585,45.80488618,45.80452718,45.80431776,45.80422801,45.80416818,45.80407843,45.80407095,45.80417566,45.80454214,45.80520778,45.80590333,45.80672601,45.80736171,45.80783287,45.80841621,45.8088051",
				"45.78820505,45.78789083,45.78781602,45.78789083,45.7879058,45.78798061,45.78801053,45.78841453,45.78890829,45.78914769,45.78940205,45.7904943,45.7903746,45.79029979,45.79029979,45.78940205,45.78929732,45.78929732,45.78937213,45.7897013,45.78974619,45.78976115,45.79000055,45.79019505,45.79040452,45.79043445,45.79107782,45.79149675,45.79167629,45.79420477,45.79417485,45.79401028,45.79393547,45.79399532,45.79407012,45.79136209",
				"45.80947811,45.81006143,45.81092892,45.81173657,45.81248439,45.81249935,45.81253674,45.81283586,45.81318738,45.81326964,45.81356128,45.81291817,45.81247696,45.81237975,45.81210306,45.81175906,45.81143002,45.81104115,45.81072706,45.81037558,45.81003905,45.8098147,45.80978468,45.80971737,45.80963511,45.80956033",
				"45.82410419,45.82418643,45.82375279,45.82369298,45.82351354,45.82335653,45.82327429,45.82306494,45.82292288,45.8228855,45.82327429,45.82325933,45.82364812",
				"45.81843666,45.81879557,45.81897502,45.81936384,45.81920682,45.81898998,45.818818",
				"45.81345648,45.81390516,45.81483989,45.81543811,45.8157746,45.81593163,45.81585686,45.81566244,45.8152063,45.8146006,45.81414445,45.81370325,45.81368082,45.81356865",
				"45.79410025,45.79413766,45.79420498,45.79432467,45.79356913,45.79255176,45.79248443,45.79230489,45.79217023,45.79215527,45.79215527,45.79214031,45.79110047,45.79026259,45.79016534,45.79010549,45.79022519,45.79082367,45.79110795,45.79143711,45.79255924,45.79344944,45.79418254,45.79457901,45.79491563,45.795155,45.79540934,45.79550658,45.79539438,45.79534949,45.79528965,45.79528965,45.79522981,45.79478098,45.79410773",
				"45.77725145,45.7775807,45.77780518,45.77784259,45.77784259,45.77784259,45.77743104,45.77707187,45.77623378,45.77617392,45.77608412,45.77604671,45.77605419,45.77605419,45.77655555,45.77708683,45.77708683,45.77712425",
				"45.8019841,45.80276945,45.80287416,45.80288912,45.80299383,45.80310602,45.80319577,45.80264978,45.80239547,45.80196914,45.80193923,45.80186443,45.80186443,45.80192427,45.80194671",
				"45.78933488,45.78931244,45.78929747,45.78997826,45.79104057,45.79105553,45.79109293,45.79115278,45.79119019,45.789881,45.78953687",
				"45.83210337,45.83211832,45.83208842,45.83208842,45.83205105,45.83168475,45.83137825,45.83115398,45.83085496,45.83081758,45.83081758,45.83080263,45.83082506,45.83086243,45.83113903",
				"45.82756571,45.82755824,45.82751338,45.82743862,45.82734143,45.82715453,45.82701248,45.8267882,45.82675082,45.82661625,45.82645177,45.82607048,45.82517332,45.82498641,45.82519575,45.82524808,45.82539014,45.8256219,45.82626486,45.8264742,45.82701248",
				"45.82847026,45.82835064,45.82785723,45.82725167,45.82687038,45.82636948,45.82586109,45.82534525,45.8248219,45.82438827,45.82402192,45.82278828,45.82138256,45.8199321,45.81945356,45.81923672,45.81910214,45.81888532,45.81861614,45.81856377,45.81862359,45.81878809,45.81953581,45.8203882,45.82090411,45.82159199,45.82222004,45.82227241,45.82238456,45.82260138,45.82265372,45.82265372,45.82262381,45.82250419,45.82242194,45.8223397,45.82231727,45.82253409,45.82271353,45.82292288,45.82311728,45.82348361,45.8239322,45.82426864,45.82471723,45.82512096,45.82562188,45.82618992,45.82670578,45.8271693,45.82775243,45.82799167,45.828044,45.82810381,45.82826828",
				"45.8141969,45.81404734,45.81398004,45.81390526,45.8137557,45.81378561,45.81378561,45.81378561,45.81380057,45.81378561,45.81378561,45.81378561,45.81379309,45.81380805,45.81398004,45.81418942,45.81450349,45.81476521,45.81492972,45.8151316,45.81548305,45.8158345,45.8162009,45.81652992,45.81679148,45.81704571,45.81744202,45.81764391,45.81792057,45.81816732,45.81831689,45.81878796,45.81928146,45.81967027,45.8199544,45.82038807,45.82047032,45.82053013,45.82008899,45.81986468,45.81958802,45.81930389,45.81903471,45.81857862,45.81821224,45.81793558,45.8177636,45.81764396,45.81737477,45.81718036,45.81692612,45.81662702,45.81613353,45.81569982,45.81533342,45.81495956,45.81480252,45.81463053,45.81454828,45.81442863,45.81430151",
				"45.8140099,45.81397251,45.81389025,45.81417441,45.81456326,45.81475768,45.81501941,45.81524374,45.81539329,45.8155578,45.81580456,45.81613358,45.81653721,45.81721019,45.81759156,45.81782336,45.81795048,45.81805516,45.81820471,45.81839912,45.81864587,45.81907204,45.81920663,45.81949824,45.81987958,45.82012632,45.82027586,45.82025343,45.82010389,45.8198721,45.81945338,45.81933375,45.81930384,45.81945338,45.81992444,45.82027586,45.82062728,45.82077682,45.82124787,45.82153199,45.8218535,45.82221986,45.82247407,45.82295258,45.82319931,45.82342361,45.82314698,45.82295258,45.82254136,45.82208528,45.82184602,45.8215619,45.82138993,45.82126282,45.82114319,45.82147965,45.82183107,45.82203294,45.82221238,45.82242921,45.82267594,45.82290772,45.82311712,45.82325918,45.82361058,45.82385731,45.82374516,45.82365544,45.82361805,45.8238274,45.82393955,45.82402179,45.82405917,45.82417132,45.82423861,45.82428347,45.82464982,45.82496388,45.82544984,45.82624233,45.82690024,45.82681052,45.82664605,45.82655633,45.82640681,45.8262199,45.826033,45.82587589,45.82566655,45.8253974,45.82500871,45.82444042,45.8241937,45.82386473,45.82357314,45.82338623,45.82325165,45.82316193,45.82318436,45.82331894,45.82361053,45.82383493,45.82369287,45.82361063,45.82342371,45.82335642,45.82337885,45.82340128,45.82317699,45.82286296,45.82274334,45.82263119,45.82219003,45.8217489,45.82109841,45.82044791,45.81973754,45.8196179,45.8189898,45.81878791,45.81854864,45.81839909,45.81801775,45.8174719,45.81691109,45.81632036,45.81587171,45.81552773,45.81531849,45.81522883,45.81516901,45.81506432,45.81496711,45.81487738,45.81469044,45.81447358,45.8141969,45.81391274,45.81364353,45.81345658,45.81333694,45.81327711,45.81312753,45.81294806,45.81276858,45.81246946,45.81223016,45.81202825,45.8117441,45.81141506,45.81121305,45.81094383,45.81060741,45.81036062,45.81022593,45.81002402,45.80996419,45.80981462,45.80970244,45.80958279,45.80941826,45.8092986,45.80923878,45.80997164,45.81078678,45.81166922,45.81198338,45.8122152,45.81246198,45.81275363,45.81310509,45.81351638,45.81398001,45.81412202,45.81441365,45.81486232,45.815139,45.81528855,45.81579703,45.81674669,45.81658218,45.81637281,45.81611112,45.81592418,45.81571481,45.81528118,45.8150718,45.81492222,45.81475771,45.81470536,45.81482501,45.81478014,45.81470536,45.8145932,45.81444364,45.81424922",
				"45.81400995,45.81386787,45.81371831,45.81359867,45.81351641,45.81346406,45.81326964,45.81316494,45.81307521,45.81290321,45.812806,45.8124994,45.81267887,45.81343415,45.81416699,45.81436889,45.81514658,45.815842,45.81651499,45.81700103,45.81754688,45.81787589,45.81835444,45.81837687,45.81834696,45.81821237,45.81792075,45.81766652,45.81734499,45.81675427,45.81629814,45.81596912,45.81559524,45.81514658,45.81487738,45.8145334,45.81433898,45.81403239,45.81455583,45.81510171,45.81560272,45.81603642,45.81627571,45.81667949,45.81710571,45.81753193,45.81801796,45.81862362,45.81925918,45.81975267,45.82012653,45.82036579,45.82069478,45.82112097,45.82171164,45.82228736,45.82225745,45.82187614,45.82149482,45.82100134,45.82053029,45.8201041,45.81928161,45.81894514,45.81848155,45.81813759,45.81789832,45.81759922,45.81729265,45.81688886,45.81651499,45.81608129,45.81580462,45.81575227,45.81577471,45.81566254,45.81531857,45.81486242,45.81438384,45.81413708,45.81396508,45.81400995,45.81387535,45.81371084,45.81357623,45.81352389,45.81354632,45.81359867,45.81364353,45.81381553,45.81392769,45.813995",
				"45.80901447,45.80904439,45.80909674,45.80919396,45.80931361,45.80947066,45.80951553,45.80984448,45.81037545,45.81083911,45.81148224,45.81242449,45.81250675,45.81252918,45.81255161,45.81255909,45.81255909,45.81254414,45.81263385,45.81279089,45.81300027,45.81320966,45.81324702,45.81350127,45.81377795,45.8140995,45.81484729,45.81489963,45.81508658,45.81503423,45.81481738,45.81454818,45.81420419,45.8139649,45.81365083,45.81335919,45.81330684,45.81330684,45.81330684,45.81328441,45.81282825,45.81200566,45.81126532,45.810712,45.81001651,45.80949302",
				"45.79778041,45.79784025,45.79787765,45.79799733,45.79807213,45.79798237,45.79778789,45.79748869,45.79739893,45.79733909,45.7972568,45.79713712,45.79709224,45.79702492,45.79693516,45.79689776,45.7972568",
				"45.80881245,45.80821416,45.80778787,45.80736914,45.80679327,45.8064642,45.80607532,45.80581356,45.80561157,45.80531989,45.80509552,45.80505064,45.80497585,45.80490854,45.80478137,45.80467666,45.80422801,45.80407843,45.80389145,45.80399616,45.80432524,45.80426538,45.80415314,45.80409331,45.80401109,45.80372688,45.8035399,45.80403353,45.8041158,45.8041233,45.80423549,45.80438508,45.80460197,45.80509557,45.80531976,45.80588069,45.80642665,45.80671095,45.8071522,45.80734665,45.80756353,45.80790008,45.80817679,45.80838619,45.80864046,45.80881995,45.80891717,45.80893961,45.80894709,45.80883491,45.80873769,45.80869281,45.80878256,45.80884239,45.80895451,45.80906669,45.80911904,45.80914148,45.80891712,45.80858806,45.80843852,45.80819172,45.80791501,45.80781031,45.80711478,45.80701756,45.80694277,45.80670344,45.80597061,45.80543961,45.80457951,45.80383907,45.80305366,45.8023132,45.80186443,45.80133338,45.80122872,45.80114644,45.80098189,45.80086222,45.80070512,45.80054057,45.80030122,45.80009178,45.79985243,45.79972527,45.7995308,45.79914192,45.79879785,45.79876793,45.79879785,45.79879785,45.79882777,45.79864077,45.79846125,45.79838645,45.79830417,45.79821441,45.79809473,45.79799001,45.79772073,45.79751876,45.79726439,45.7969951,45.79646401,45.79591795,45.7949829,45.79455654,45.79439197,45.79438449,45.79437701,45.7943396,45.7943396,45.7943695,45.7943695,45.79433958,45.79430218,45.7942947,45.79426477,45.79424981,45.79423485,45.79428724,45.79434708,45.79441441,45.79449669,45.79463132,45.79475101,45.79477345,45.79482581,45.79483329,45.795327,45.79579078,45.79582818,45.79582818,45.79582818,45.79547661,45.79522975,45.79501282,45.79499038,45.79501282,45.79508757,45.79508757,45.79507272,45.79505028,45.79509516,45.795155,45.79522233,45.79532703,45.79537191,45.79544671,45.79551403,45.79557385,45.79564118,45.79572346,45.79587309,45.79600026,45.79615734,45.79620222,45.79632939,45.79650146,45.79658374,45.79671828,45.7967482,45.79683797,45.79698009,45.7973915,45.79772062,45.79828165,45.79882021,45.79944104,45.80025642,45.80104178,45.80157283,45.80202158,45.80270221,45.80369696,45.8039363,45.80476649,45.80550692,45.80623238,45.8067109,45.80745878,45.80831135,45.80875259",
				"45.77343506,45.77341261,45.77339764,45.77376432,45.77425822,45.77487184,45.77543307,45.77592695,45.77652559,45.77653307,45.77652559,45.77650314,45.77653307,45.77684736,45.77735619,45.77771536,45.77775278,45.77777522,45.77778271,45.77785753,45.77786502,45.77792488,45.77789495,45.77762557,45.77718408,45.77705688,45.7769147,45.77627865,45.77593444,45.77570246,45.7756875,45.77565756,45.77562763,45.7755977,45.77556777,45.77556029,45.77550042,45.77547049,45.77490177,45.77419087",
				"45.77718408,45.77730381,45.77734122,45.77731877,45.77734871,45.77734871,45.77715415,45.77692218,45.7766079,45.77642083,45.77614396,45.77585961,45.77579226,45.77578478,45.77575484,45.77574736,45.77572491,45.7757324,45.77574736,45.77612899,45.77661539",
				"45.79270885,45.79270885,45.79274625,45.79271633,45.79267145,45.79247695,45.79231237,45.79217023,45.79211039,45.79213283,45.79213283,45.79207298,45.79186352,45.79174383,45.79163161,45.79152688,45.79148948,45.79148948,45.79147451,45.79144459,45.79145207,45.79130245,45.79112291,45.7910107,45.79086108,45.79075634,45.79058428,45.79048702,45.7904571,45.79064413,45.79093589,45.79127253,45.79148948,45.79153436,45.79166902,45.79205802,45.79242458",
				"45.79486327,45.79483334,45.79519988,45.79555894,45.7958806,45.7959554,45.7959554,45.7959928,45.79597036,45.7959928,45.7958432,45.79559635,45.79532705,45.79520737,45.79513256,45.79499791,45.79495303,45.79493807",
				"45.801685,45.8017598,45.80190191,45.80190191,45.80190939,45.80235068,45.80294903,45.80330804,45.80336039,45.80354738,45.80373436,45.80372688,45.80362965,45.80358477,45.80339031,45.80298643,45.8025601,45.80216369,45.80189443,45.8017598,45.80170744,45.80165508",
				"45.80439993,45.80428026,45.80411572,45.80398857,45.80319577,45.8027844,45.80279936,45.80283676,45.80286668,45.80314341,45.80330796,45.8036894,45.80404093",
				"45.80348741,45.80365196,45.8037791,45.80400348,45.80411567,45.80422038,45.80434752,45.80437744,45.80431013,45.80419794,45.80387633",
				"45.81544569,45.8154756,45.81551299,45.81561767,45.81578218,45.81600651,45.8164103,45.81673184,45.81706085,45.81734499,45.81774877,45.81803291,45.81836191,45.81863857,45.81885541,45.81904234,45.81953584,45.81990969,45.82034336,45.82086675,45.82115088,45.82122565,45.82121069,45.82103872,45.82071721,45.82038075,45.82004428,45.81970781,45.81937882,45.81908721,45.81879559,45.81858623,45.81831705,45.81811516,45.81784598,45.81759175,45.81715805,45.81686643,45.81661967,45.81635048,45.81608877,45.81578218",
				"45.78468143,45.78449438,45.78427741,45.78419511,45.78406043,45.78396317,45.78377612,45.78363396,45.78379109,45.78394072,45.78409036,45.78424748,45.78441957,45.78454676",
				"45.78374619,45.78359655,45.78350677,45.78346936,45.78343195,45.78337958,45.78337209,45.78334217,45.78328979,45.78323742,45.7831626,45.78304289,45.78290073,45.7827885,45.78268375,45.78261641,45.78259396,45.78260893,45.78260144,45.78246676,45.78201035,45.78179337,45.78160631,45.78159135,45.78183826,45.78231712,45.78257151,45.78284087,45.78292317,45.78293814,45.78289324,45.78288576,45.78287828,45.78293065,45.78305037,45.78320749,45.78337958,45.78351425,45.78367886,45.78373871,45.78379857",
				"45.81332198,45.81330703,45.81329955,45.8133145,45.81414455,45.81421185,45.8142642,45.81439132,45.81448853,45.81452592,45.81427168,45.81417446,45.81409969,45.81388283,45.81383048,45.81379309,45.81349397,45.81344911,45.81335189,45.81334442,45.81349397,45.81358371,45.81383796,45.81445115,45.81489234,45.81565506,45.81655985,45.81723283,45.81795066,45.81861614,45.81939377,45.81972277,45.82017887,45.82103872,45.82175651,45.82260886,45.82328924,45.82408176,45.82497146,45.82572657,45.82645177,45.82764047,45.8281937,45.82920295,45.82973373,45.83096722,45.83209603,45.83299308,45.83435357,45.83501138,45.83527301,45.83577383,45.83664092,45.83680537,45.83680537,45.83663345,45.83656617,45.836469,45.83598313,45.83557948,45.83525806,45.83498896,45.83477965,45.83412932,45.8335014,45.83273144,45.83196894,45.83132605,45.8310644,45.83060839,45.83007014,45.82974868,45.82918052,45.82855255,45.82793204,45.82743114,45.82690034,45.82635458,45.82568919,45.82509108,45.82438082,45.82361073,45.82315466,45.82245185,45.82164435,45.8211434,45.82055272,45.81995456,45.81961809,45.81954331,45.81920684,45.81896009,45.81873578,45.81844416,45.81812264,45.81786093,45.81755436,45.81720292,45.81685148,45.81614111,45.81591678,45.81550551,45.81539334,45.81514658,45.81498955,45.81452592,45.81387535",
				"45.81506432,45.8148699,45.81467548,45.81450349,45.8143988,45.81434646,45.81434646,45.81436141,45.81437637,45.81438384,45.81465305"

			};
			#endregion
			#region ylist
			String[] yList = {
				"15.98888405,15.98933466,15.98976381,15.99004276,15.99032171,15.99036463,15.99036463,15.99040754,15.99040754,15.99041827,15.990429,15.99041827,15.98999985,15.98955996,15.9892381,15.98912008,15.98900206,15.98888405,15.9888304,15.98880894,15.9888304",
				"15.97769376,15.97811218,15.97844478,15.97904559,15.97960349,15.97967859,15.97967859,15.97970005,15.97972151,15.97976442,15.97974297,15.97980734,15.9798288,15.97985026,15.9798288,15.97980734,15.97976442,15.97973224,15.97970005,15.97957131,15.97931381,15.97882029,15.97831603,15.97802635,15.97790834,15.97785469,15.97777959,15.97772595,15.97769376",
				"15.96576333,15.96517324,15.96464753,15.96413255,15.96408963,15.96411109,15.9640789,15.96410036,15.96413255,15.96413255,15.96557021,15.96614957,15.96618176,15.96620321,15.96617103,15.9660852,15.96596718,15.96590281,15.9658277,15.96578479,15.96576333",
				"15.95706224,15.95714808,15.95727682,15.95738411,15.95745921,15.9575665,15.95837116,15.95905781,15.95969081,15.96071005,15.96178293,15.96276999,15.963521,15.96450806,15.96443295,15.96413255,15.96387506,15.9643364,15.96530199,15.96620321,15.96697569,15.96722245,15.96745849,15.9675765,15.96756577,15.96699715,15.96644998,15.96580625,15.96473336,15.96467972,15.96478701,15.96503377,15.9651196,15.9656024,15.96584916,15.96589208,15.96592426,15.96563458,15.96635342,15.96738338,15.96857429,15.96976519,15.9695828,15.96941113,15.96955061,15.96985102,15.97082734,15.97060204,15.97032309,15.97033381,15.97073078,15.97085953,15.97123504,15.9713316,15.97273707,15.97343445,15.97432494,15.97515106,15.97577333,15.9761703,15.97636342,15.97635269,15.97623467,15.97606301,15.97574115,15.97573042,15.97573042,15.9756875,15.9756875,15.97558022,15.97560167,15.97563386,15.97533345,15.97533345,15.97605228,15.97621322,15.97641706,15.97662091,15.97718954,15.97740412,15.977844,15.97816586,15.97843409,15.97834826,15.97820878,15.97800493,15.97792983,15.97838044,15.97873449,15.97906709,15.97980738,15.98053694,15.9807837,15.98168492,15.98193169,15.9819746,15.98201752,15.98274708,15.98346591,15.98362684,15.98365903,15.98377705,15.98377705,15.98362684,15.98353028,15.9829402,15.98255396,15.9824574,15.98248959,15.98258615,15.98269343,15.98325133,15.98350883,15.98368049,15.98403454,15.98406672,15.98408818,15.98402381,15.98390579,15.98345518,15.98310113,15.98317623,15.98326206,15.98329425,15.98276854,15.98326206,15.9834981,15.9842062,15.98500013,15.98570824,15.98717809,15.98947406,15.99060059,15.99207044,15.99337935,15.99487066,15.9965229,15.99759579,15.99814296,16.00020289,16.0025847,16.00449443,16.00482702,16.00508451,16.00530982,16.00565314,16.00639343,16.00470901,16.00372195,16.00242376,16.0016942,16.00040674,16.00036383,16.00062132,16.00122213,16.00205898,16.0026598,16.00311041,16.00346446,16.00073934,15.99894762,15.99374413,15.99297166,15.99053621,15.98929167,15.98893762,15.98685622,15.98139524,15.97748995,15.97531199,15.9737134,15.97060204,15.96908927,15.96802711,15.96668601,15.9652698,15.96417546,15.9629631,15.96198678,15.96116066,15.95993757,15.95892906",
				"15.95468059,15.95586076,15.95618263,15.95667616,15.95714822,15.95781341,15.95796362,15.95798507,15.95789924,15.95787778,15.9572126,15.95804945,15.95892921,15.96004501,15.96038833,15.96051708,15.96053854,15.96214786,15.96249118,15.96525922,15.96620336,15.96646085,15.96776977,15.96912161,15.96980825,15.97034469,15.96991554,15.9696795,15.9693791,15.96916452,15.96719041,15.96437946,15.96246973,15.96045271,15.95875755,15.95671907",
				"15.94847914,15.94842549,15.94845768,15.94852205,15.94867226,15.94691273,15.94664451,15.94543215,15.94388731,15.94332941,15.94202049,15.94197758,15.9419132,15.94187029,15.94193466,15.94192393,15.9419132,15.94193466,15.94193466,15.94192393,15.94190247,15.9418381,15.94248172,15.94365112,15.94521753,15.94679467",
				"16.01295948,16.01396799,16.01406455,16.01406455,16.01396799,16.0137856,16.01367831,16.01329207,16.0130024,16.01289511,16.01278782,16.01295948,16.01295948",
				"16.01308808,16.01459011,16.01545915,16.01555571,16.01480469,16.01393566,16.01305589",
				"16.01956844,16.01849556,16.01615667,16.01719737,16.01817369,16.01915002,16.01924658,16.01937532,16.01950407,16.01970792,16.0199976,16.020298,16.02037311,16.02001905",
				"15.9916842,15.99290729,15.99422693,15.99572897,15.99582553,15.99576116,15.99476337,15.99476337,15.99642634,15.99773526,15.99822879,15.99849701,15.99843264,15.99840045,15.99956989,16.00112557,16.00208044,16.00193024,16.0016942,16.00259542,16.00165129,16.00087881,16.00022435,15.99978447,15.99858284,15.99776745,15.99694133,15.99631906,15.99568605,15.99483848,15.99316478,15.99185586,15.99155545,15.99138379,15.99181294",
				"15.9956646,15.99567533,15.99568605,15.9962225,15.99675894,15.9968555,15.99687696,15.99691987,15.99691987,15.99691987,15.99664092,15.99601865,15.99517107,15.99506378,15.99502087,15.99499941,15.99534273,15.99552512",
				"15.89780673,15.89780673,15.89779601,15.8982788,15.89908347,15.90023145,15.90120777,15.90131506,15.90139017,15.90132579,15.90129361,15.90129361,15.90085372,15.89923367,15.89838609",
				"15.93032587,15.92948902,15.92792261,15.92792261,15.92784751,15.92857707,15.9291457,15.92989672,15.93005765,15.93030442,15.93034733",
				"16.0560035,16.05642192,16.05722658,16.05857842,16.05883591,16.05883591,16.05881445,16.05881445,16.05881445,16.05879299,16.05819218,16.05729096,16.05666868,16.05597131,16.05602495",
				"16.10650435,16.10708371,16.10810295,16.10870376,16.10941187,16.11049548,16.11121431,16.1120297,16.11207262,16.11198679,16.11194387,16.11179367,16.11135378,16.11133233,16.11040965,16.11007705,16.10947624,16.10851064,16.10618249,16.10612884,16.10631123",
				"15.9794854,15.97960342,15.97977508,15.97993601,15.9800433,15.98021496,15.98038662,15.98054763,15.98059054,15.98063346,15.98051544,15.98010775,15.97982869,15.97960342,15.97951755,15.97950682,15.97943172,15.97905643,15.97857364,15.97812299,15.97792991,15.97798355,15.97804792,15.97810157,15.97820885,15.9783376,15.9784127,15.97829461,15.97806931,15.97787619,15.97754359,15.97729683,15.97705021,15.97658888,15.97619191,15.97571984,15.97561255,15.97563401,15.9758915,15.97617045,15.97635284,15.97648151,15.97668536,15.97684629,15.9768999,15.97693209,15.97708229,15.97712532,15.97714677,15.97736135,15.97747937,15.97761884,15.97805873,15.97846642,15.97894922",
				"15.98318681,15.98331556,15.98340139,15.9837769,15.98438844,15.98477468,15.98517165,15.98553643,15.98584756,15.98611578,15.98645911,15.98674879,15.98697409,15.9872745,15.98725304,15.98721013,15.98713502,15.98707065,15.98707065,15.98693118,15.98678097,15.98663077,15.98649129,15.98635182,15.98637328,15.98640546,15.98649129,15.98657712,15.98658785,15.98656639,15.98653432,15.9861159,15.9860836,15.98611578,15.98610505,15.98616943,15.98622307,15.98597631,15.98571882,15.98509654,15.98515019,15.98515019,15.98513946,15.9849678,15.98478541,15.98464593,15.98451719,15.98434553,15.98425969,15.98424897,15.98423824,15.98420605,15.98420613,15.98414175,15.98405592,15.98403439,15.98398075,15.98389491,15.98379835,15.98351941,15.98326191",
				"15.99323984,15.99208113,15.99052545,15.99053618,15.99060055,15.99067565,15.99130865,15.99194165,15.99253174,15.99306818,15.99390503,15.99446293,15.99517103,15.99604007,15.99658724,15.99733826,15.99803563,15.99866863,15.9995484,16.00021359,16.00092169,16.00200523,16.00187648,16.00154389,16.00123275,16.00105036,16.00139368,16.00149024,16.0015868,16.00179065,16.00209106,16.00225199,16.00231636,16.00263823,16.00361455,16.00444067,16.00515951,16.00567449,16.00500938,16.00452658,16.0042369,16.00373264,16.00333568,16.00273486,16.00250956,16.00236997,16.00220904,16.00216612,16.00211248,16.00203738,16.00201592,16.00201592,16.00205883,16.00209102,16.00203738,16.00176916,16.00135073,16.00096449,16.0006319,16.00019202,15.99967703,15.99931225,15.99908706,15.99893685,15.99867936,15.99853989,15.99819657,15.99798199,15.99785324,15.99769231,15.99755284,15.99748846,15.99774595,15.99805709,15.99825021,15.99836823,15.99817511,15.99808928,15.9980249,15.99790689,15.99776741,15.99844333,15.99923726,15.99990245,16.00035306,16.00109335,16.00178,16.00220908,16.00241292,16.00261677,16.00306746,16.00385059,16.00409735,16.00464452,16.00513805,16.00568522,16.0062753,16.00670446,16.0073911,16.00805629,16.00895751,16.00968711,16.01021282,16.01058833,16.01103895,16.01150029,16.01220839,16.01227276,16.01234786,16.01242297,16.01245515,16.0125088,16.01246581,16.01251945,16.01262674,16.01277694,16.01290584,16.01297021,16.01307742,16.01307742,16.0130667,16.0130667,16.01304524,16.01303451,16.01301305,16.01305597,16.0131418,16.01322763,16.01319548,16.01395715,16.01434339,16.01469744,16.01511587,16.01544846,16.01607073,16.01663936,16.01732601,16.01799119,16.01853837,16.01892457,16.01884946,16.01880655,16.01823807,16.01754069,16.01684332,16.01590991,16.01495505,16.01430059,16.01318475,16.01197239,16.01119995,16.01027727,16.00902196,16.00803491,16.00767013,16.00675818,16.00631829,16.00567456,16.00517031,16.00456949,16.00388285,16.00343224,16.00318547,16.00231644,16.00140449,16.00039598,16.00042805,16.00117907,16.00196227,16.00276694,16.00371107,16.00485906,16.00597486,16.00625392,16.00589987,16.00549217,16.00519177,16.00513812,16.00492358,16.00456953,16.00373268,16.00286365,16.00183353,16.00105032,16.00011691,15.99816434,15.99745624,15.9969734,15.99650133,15.99637259,15.99622238,15.99602923,15.99567518,15.99535331,15.99488124,15.99421605",
				"15.95765233,15.95781326,15.95798492,15.95813513,15.95839262,15.95866084,15.9586072,15.95900416,15.95959425,15.96055984,15.96112847,15.96270561,15.96440077,15.96406817,15.96375704,15.96436858,15.96388578,15.96368194,15.96356392,15.96349955,15.96360683,15.96375704,15.96399307,15.96354246,15.96330643,15.96315622,15.96304893,15.96294165,15.96285582,15.96285582,15.96290946,15.96294165,15.96302748,15.96310258,15.96325278,15.96348882,15.96363902,15.96319914,15.96267343,15.96224427,15.96181512,15.96135378,15.96118212,15.96119285,15.96122503,15.96122503,15.96125722,15.9611392,15.96078515,15.96057057,15.9604311,15.96037745,15.96033454,15.96034527,15.96020579,15.96009851,15.95981956,15.95991611,15.96001267,15.96005559,15.96007705,15.96015215,15.96047401,15.96063495,15.96077442,15.96089244,15.96094608,15.96097827,15.96097827,15.96093535,15.96087098,15.96079588,15.96072078,15.96063495,15.96033454,15.95999122,15.9599483,15.95983028,15.95977664,15.95952988,15.95934749,15.95886469,15.95878959,15.95873594,15.95871449,15.9586823,15.95856428,15.95833898,15.95818877,15.9579742,15.95787764,15.95779181",
				"15.95706217,15.95613949,15.95494859,15.95299594,15.95104329,15.94857566,15.94838254,15.94840419,15.94839346,15.94840419,15.94845783,15.94858658,15.94857585,15.94886553,15.94920885,15.95015299,15.95086109,15.95212709,15.95304959,15.95384352,15.95473401,15.95574252,15.95581766,15.95571037,15.9554207,15.95492717,15.95392939,15.95387574,15.95406886,15.95425125,15.95446583,15.95484134,15.95536705,15.95578548,15.95606443,15.956279,15.95633265,15.95670816,15.95715877,15.95744845,15.95738407,15.95734116,15.9572446,15.95720164,15.957148,15.95708363",
				"15.94801772,15.94841469,15.94892968,15.94947685,15.9498309,15.949906,15.94994891,15.95004547,15.95006693,15.95008839,15.94997037,15.94969142,15.94932664,15.9489404,15.94857562,15.94832886,15.94817866",
				"16.00338928,16.00122206,15.99961273,15.99811085,15.99587925,15.99435575,15.99290743,15.99202767,15.99134102,15.99030033,15.98937765,15.98907724,15.98831549,15.98715678,15.98578338,15.98441008,15.98357331,15.9827901,15.98042976,15.97937833,15.9773935,15.97575188,15.97408891,15.97271562,15.97194318,15.97067717,15.96971158,15.969497,15.969497,15.96887473,15.96804861,15.96743707,15.96665386,15.96554875,15.96510898,15.96427213,15.96342455,15.96297394,15.96227657,15.96202981,15.96155774,15.96094619,15.96042048,15.95983039,15.95918666,15.95859647,15.95788836,15.95740556,15.95699787,15.95698714,15.95692277,15.95679417,15.95562473,15.95425144,15.95237385,15.95133316,15.95054995,15.95018517,15.95022809,15.9501959,15.95062502,15.95070012,15.95078595,15.95102198,15.95118292,15.95126875,15.95170863,15.95245965,15.9525777,15.95269572,15.95285665,15.95300685,15.95318921,15.95328577,15.95340378,15.95345743,15.95296398,15.95272794,15.95256701,15.95243826,15.95259905,15.95283508,15.95314622,15.95350027,15.95377922,15.95401525,15.95407963,15.95416553,15.95424063,15.95434792,15.95470197,15.9551204,15.95565684,15.95566757,15.95566757,15.95577486,15.95602162,15.95636494,15.95668681,15.95695503,15.9576524,15.95833905,15.95930472,15.95909014,15.95871463,15.95830694,15.95766321,15.95735207,15.95724478,15.95779195,15.95830694,15.9589614,15.9598948,15.96079588,15.96143961,15.96200824,15.9628129,15.96368193,15.9652698,15.96604228,15.9668684,15.96822027,15.96932534,15.97037677,15.97179297,15.9745181,15.97546231,15.97608458,15.97662102,15.97678196,15.97672831,15.97679268,15.97725402,15.97789775,15.97872387,15.97876679,15.97876679,15.97879898,15.97903501,15.97938906,15.98045118,15.98118074,15.98209269,15.98270424,15.98371275,15.9846998,15.9864486,15.98828331,15.98928109,15.99049345,15.99182382,15.99376574,15.9949781,15.99675909,15.99965587,16.00184456,16.0048591,16.00617874,16.00859284,16.01197235,16.01376407,16.01577044,16.01669312,16.01792693,16.01765871,16.01684332,16.01622105,16.01544842,16.01481542,16.01404294,16.01315256,16.0122728,16.01166125,16.01117842,16.01034157,16.00921504,16.00898966,16.00804552,16.00720868,16.00641474,16.00589983,16.00507371,16.00410812,16.00361459",
				"15.97205043,15.97127795,15.97053766,15.97037673,15.970366,15.97033381,15.97028017,15.97026944,15.97028017,15.9696579,15.96887469,15.96832752,15.96811294,15.96774816,15.96699715,15.96810222,15.96882105,15.9695828,15.97075224,15.97191095,15.97404599,15.9761703,15.97683549,15.97723246,15.97790837,15.97805858,15.97808003,15.97811222,15.97812295,15.97813368,15.97787619,15.97724318,15.97630978,15.97556949,15.97472191,15.97403526,15.97280145,15.97198606,15.97197533,15.97201824",
				"15.98067649,15.9807945,15.98118074,15.98184593,15.98244675,15.98294027,15.98290808,15.98285444,15.98285444,15.9828759,15.98288663,15.98291881,15.98291881,15.98263986,15.98226435,15.98165281,15.98119147,15.98081596,15.98066576,15.98065503,15.9806443",
				"15.96928239,15.96961498,15.9701407,15.97040892,15.97048402,15.97060204,15.97072005,15.97085953,15.97089171,15.97138524,15.97177148,15.97182512,15.97180367,15.97180367,15.97179294,15.97175002,15.97162127,15.97139597,15.97109556,15.9707737,15.97050548,15.97047329,15.97045183,15.97046256,15.97045183,15.97046256,15.97043037,15.97038746,15.97033381,15.97019434,15.96997976,15.96978664,15.96964717,15.96959352,15.96936822,15.96930385,15.96928239",
				"15.97685684,15.97670663,15.97670663,15.97670663,15.97670663,15.97682465,15.97733963,15.97791899,15.97833741,15.97874511,15.9787773,15.97879875,15.97879875,15.97880948,15.97880948,15.97875584,15.97845543,15.97789753",
				"15.97850919,15.97815514,15.97779036,15.97740412,15.9772861,15.97727537,15.97722173,15.97718954,15.97787619,15.9784019,15.97867012,15.97924948,15.97960353,15.97964644,15.9796679,15.97960353,15.97953916,15.97952843,15.97950697,15.97949624,15.97952843,15.97913146",
				"15.99182352,15.99093303,15.99019274,15.9897314,15.99020347,15.99065408,15.99119052,15.99184498,15.99223122,15.99216685,15.99222049,15.99209175,15.99199519",
				"15.99433418,15.99464532,15.9950101,15.99513885,15.99517103,15.99467751,15.99416252,15.99402305,15.99388357,15.99391576,15.99408742",
				"15.96819878,15.96808076,15.9679842,15.9679842,15.96804857,15.96814513,15.96828461,15.96841335,15.9685421,15.96862793,15.96879959,15.96886396,15.96900344,15.96908927,15.96904635,15.96906781,15.9690249,15.96899271,15.96897125,15.96891761,15.96891761,15.96890688,15.9691751,15.96922874,15.96927166,15.96928239,15.96929312,15.96933603,15.96934676,15.96937895,15.96940041,15.96936822,15.96928239,15.96922874,15.96912146,15.9690249,15.96887469,15.96875668,15.96866012,15.9685421,15.96844554,15.96832752",
				"15.95024943,15.95051765,15.95020652,15.95005631,15.94988465,15.94970226,15.94939113,15.94906926,15.94881177,15.94900489,15.94928384,15.94957352,15.94984174,15.95005631",
				"15.98081589,15.9807837,15.98073006,15.98086953,15.98122358,15.98165274,15.98200679,15.98243594,15.98273635,15.98315477,15.98373413,15.98422766,15.98504305,15.9858799,15.98669529,15.98753214,15.98820806,15.98906636,15.98965645,15.98967791,15.98972082,15.98972082,15.98973155,15.99051476,15.99048257,15.99045038,15.99040747,15.99046111,15.99043965,15.98966718,15.98925948,15.98858356,15.98787546,15.9869206,15.98609447,15.98507524,15.98422766,15.98353028,15.98284364,15.98222136,15.98160982",
				"15.9702158,15.97048402,15.97080588,15.97093463,15.97106338,15.97121358,15.97172856,15.97209334,15.97265124,15.97320914,15.97349882,15.97404599,15.97470045,15.97522616,15.97570896,15.97583771,15.97606301,15.97611666,15.97620249,15.97637415,15.97630978,15.97620249,15.97604156,15.97580552,15.97576261,15.97570896,15.97567677,15.97569823,15.97562313,15.97564459,15.9753871,15.97534418,15.97537637,15.97544074,15.97555876,15.97579479,15.97584844,15.97580552,15.97585917,15.97589135,15.97589135,15.9754622,15.97547293,15.97573042,15.97596645,15.97562313,15.9754622,15.9753871,15.97518325,15.97496867,15.97450733,15.97388506,15.97313404,15.97295165,15.97258687,15.9725225,15.97262979,15.97298384,15.97334862,15.97362757,15.97392797,15.97436786,15.97468972,15.97487211,15.97494721,15.97509742,15.97510815,15.97510815,15.97509742,15.97525835,15.9754622,15.97559094,15.97537637,15.97515106,15.97496867,15.97514033,15.97537637,15.9754622,15.97551584,15.97549438,15.97544074,15.97537637,15.9754622,15.9753871,15.97519398,15.97503304,15.97500086,15.97494721,15.97492576,15.97461462,15.97433567,15.97413182,15.97430348,15.97414255,15.97335935,15.97277999,15.9721899,15.97143888,15.97098827,15.97058058,15.97038746,15.97032309,15.97049475,15.97060204,15.9706986,15.97056985,15.970366",
				"15.98181367,15.98196387,15.98225355,15.9825325,15.98267198,15.98250031,15.98221064,15.98194242,15.98170638,15.98153472,15.98163128"

			};
			#endregion

			int i,j;

			for (i = 0; i < zones.Length; i++) {
				String[] x = xList [i].Split (',');
				String[] y = yList [i].Split (',');

				zones [i] = new PolygonOptions ();

				for (j = 0; j < x.Length; j++) {
					zones [i].Add (new LatLng (double.Parse (x [j], CultureInfo.InvariantCulture), double.Parse (y [j], CultureInfo.InvariantCulture)));
				}
			}
					

			i = 0;
			foreach (PolygonOptions zone in zones) {
				zone.InvokeFillColor (zoneColors[i]);
				i++;
			}

			isInitialized = true;
		}

		public static int inZone (double latitude, double longitude){
			if (!isInitialized) {
				Initialize ();
			}

			LatLng point = new LatLng (latitude, longitude);
			List<int> zonesPointIsIn = new List<int> ();
			int targetZone = 1000;

			int i;
			for (i = 0; i < zones.Length; i++) {
				if (Contains(point,zones[i])){
					zonesPointIsIn.Add (i);
				}
			}

			if (zonesPointIsIn.Count == 0) {
				return 1000;
			} else if (zonesPointIsIn.Count == 1) {
				targetZone = zonesPointIsIn [0];
			} else if (zonesPointIsIn.Count == 2) {
				if (zonesPointIsIn.Contains (3) && zonesPointIsIn.Contains (33)) {
					targetZone = 33;
				} else if (zonesPointIsIn.Contains (21) && (zonesPointIsIn.Contains (26) | zonesPointIsIn.Contains (27) | zonesPointIsIn.Contains (28))) {
					targetZone = zonesPointIsIn [0] != 21 ? zonesPointIsIn [0] : zonesPointIsIn [1];
				} else {
					targetZone = zonesPointIsIn [0];
				}
			} else {
				foreach(int zone in zonesPointIsIn){
					if (zone != 3 && zone != 21){
						targetZone = zone;
						break;
					}
				}
			}
						
			return targetZone;
		}

		private static bool Contains(LatLng position, PolygonOptions polygon) 
		{
			List<LatLng> poly_points = new List<LatLng>(polygon.Points);

			double lat = position.Latitude;
			double lon = position.Longitude;

			bool c = false;
			int i = 0;
			int j = poly_points.Count -1;

			foreach (LatLng point in poly_points)
			{

				double lat_i = point.Latitude;
				double lon_i = point.Longitude;

				double lat_j = poly_points[j].Latitude;
				double lon_j = poly_points[j].Longitude;

				if ( ((lat_i  >  lat != (lat_j > lat)) &&
					(lon < (lon_j - lon_i) * (lat - lat_i) / (lat_j - lat_i) + lon_i) ) )
					c = !c;
				j=i++;
			}

			return c;
		}



		public static string zoneName (int i)
		{
			if (i == 1000) {
				return "unknown";
			}

			switch (zoneColors [i]) {
			case red:
				return "1";
			case darkRed:
				return "1.1";
			case yellow:
				return "2";
			case green:
				return "3";
			case cyan:
				return "4.1";
			case blue:
				return "4.2";
			default:
				return "outside";
			}
		}
	}
}

