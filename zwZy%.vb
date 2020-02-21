[IntrabarOrderGeneration = False]
inputs:
	ZsPerH1(5),ZsHcPer1(70),ZsHcPerR(30),HcKz(1);
variables:
	MP(0),ODH(0),
	FlagD(0),FlagK(0),
	ZHLD(0),ZHLK(999999),
	ZGHLD(0),ZGHLK(0),
	ZsHcPerD(999),ZsHcPerK(999),
	MzyLineD(0), MzyLineK(0);
MP = marketposition ;
ODH = entryprice*ZsPerH1*0.01;
//-----------MaxHlHcExit--------
if MP <> 1 then
	begin
	ZHLD = 0;
	flagD = 0;
	ZGHLD = 0 ;
	ZsHcPerD = 999 ;
	end;
if MP <> -1 then
	begin
	ZHLK = 999999;
	flagK = 0;
	ZGHLK = 0 ;
	ZsHcPerK = 999 ;
	end;
if MP > 0 then 
	begin
	ZHLD = MaxList(ZHLD, High) ; //开仓后价格极值-多头开仓后的最高点
	ZGHLD = ZHLD - entryprice; //最大获利绝对点数-多头
	if ZHLD > entryprice + ODH then //最大获利超过获利阈值
		begin
		
		FlagD = 1 ;
		end;
	end;
if MP < 0 then 
	begin
	ZHLK = MinList(ZHLK, Low);
	ZGHLK = entryprice - ZHLK; //最大获利绝对点数-空头
	if ZHLK <= entryprice - ODH then
		begin
		FlagK = 1 ;
		end;
	end;
//-----------MaxHlHcExit--------
inputs:
	FM(3),FZ(1),minPCS(1),LCS(0);
variables:
	PCkz(True),PCS(0);
if MP <> MP[1] then
	begin
	PCkz = True;
	end;
PCS = zwPSP(FM,FZ,minPCS,LCS);
if PCS > 0 then
	begin
	//if ExitName(1) <> "MzyPD" and MP > 0 then
	if PCkz and MP > 0 then
		begin
		if FlagD = 1 then //and ExitName(0) <> "MzyPD" 
			begin
			if ZsHcPerD > ZsHcPerR then
				ZsHcPerD  = ZsHcPer1 - ((ZGHLD - ODH)/entryprice*100) * ((ZGHLD - ODH)/entryprice*100)*3 ;
			if ZsHcPerD <= ZsHcPerR then
				ZsHcPerD  = ZsHcPerR ;
			MzyLineD = entryprice + ZGHLD * (1-ZsHcPerD*0.01);
			//text_new(date,time,MzyLineD,"+");
			//print(date," ",time," ",MP," ",entryprice," ",entryprice+ODH," ",
			//		High," ",FlagD," ",ZsHcPerD," ",MzyLineD );
			sell ("Zy%PD") PCS shares next bar at MzyLineD stop;
			{condition1 = Low <= MzyLineD point ;
			if condition1 then
				begin
				PTCpfD = False;
				ZHLD = 0;
				flagD = 0;
				ZGHLD = 0 ;
				ZsHcPerD = 999 ;
				PCkz = False;
				end;}
			end;
		end;
	//if ExitName(1) <> "MzyPK" and MP < 0 then
	if PCkz and MP < 0 then
		begin
		if FlagK = 1 then //and ExitName(0) <> "MzyPK"
			begin
			if ZsHcPerK > ZsHcPerR then
				ZsHcPerK = ZsHcPer1 - (ZGHLK - ODH) * HcKz ;
			if ZsHcPerK <= ZsHcPerR then
				ZsHcPerK = ZsHcPerR ;
			MzyLineK = entryprice - ZGHLK * (1-ZsHcPerK*0.01);
			{text_new(date,time,MzyLineK,"++");
			print(date," ",time," ",MP," ",entryprice," ",entryprice-ODH," ",
				Low," ",FlagK," ",ZsHcPerK ," ",MzyLineK );}
			buytocover ("Zy%PK") PCS shares next bar at MzyLineK stop;
			{condition1 = High >= MzyLineK point ;
			if condition1 then
				begin
				ZHLK = 999999;
				flagK = 0;
				ZGHLK = 0 ;
				ZsHcPerK = 999 ;
				PCkz = False;
				end;}
			end;
		end;
	end;
//print(date," ",time," ",PosTradeCount(0)," ",PosTradeExitName(1,PosTradeCount(0))); 


