[IntrabarOrderGeneration = False]
//----------------------参数及变量-------------------------------//
inputs:	PriceH(H),PriceL(L)；//K-bar通道计算依据
inputs:	KbCnLen1(50)；//开仓K-bar通道长度
variables:	KbCnHk(H),KbCnLk(L); //开仓K-bar通道值
inputs:	KbCnLen2(30);//平仓K-bar通道长度
variables:	KbCnHp(H),KbCnLp(L);//平仓K-bar通道值
inputs:	Mlen(120);//均线长度
variables: MA1(0); //均线
	
//------------------指标计算------------------//

Value1 = Zw_F_KBC(PriceH,PriceL,KbCnLen1,KbCnHk,KbCnLk);
Value2 = Zw_F_KBC(PriceH,PriceL,KbCnLen2,KbCnHp,KbCnLp);
MA1 = AverageFC(Close, Mlen) ;

//----------------------Open positions-------------------------------//
Input:	
	InCap(100000),MaxCap(200000),MinCap(50000),
	BSx(0.7),KCX(4), BS(0.75), KcSlAmt(1);
variables:
	MP(0), KCS(0), InCapX(0),TSxx(0),
	MaxR(0),psAlen(20),
	Myfhigh(0), Myflow(0);
//Initialize	
MP = marketposition;
//-----KCS---------//
MaxR = AvgTrueRange(psAlen);
//MaxR = (KbCnHp - KbCnLp + 2 point);
KCS = zzPSM(InCap,BSx,MaxCap,MinCap,KCX,BS,KcSlAmt,MaxR,InCapX);
//print(date," ",time," ",zwInCap(InCap,XZ,MaxInCap)," ",i_ClosedEquity);
//entry&add
if KCS > 0 then
	begin
	if MP <> MP[1] then
		TSxx = InCapX*BS*0.1/KCS/bigpointvalue;
	Myfhigh = KbCnHk + 1 point ; 
	Myflow = KbCnLk - 1 point ;
	condition1 = High <> Low; //All
	condition2 = MA1[1] > MA1[2]; //D
	condition3 = MA1[1] < MA1[2]; //K
	if condition1 then
		begin
		if condition2 then
			begin
			buy("B1") KCS shares next bar at Myfhigh stop ;
			end;
		if condition3 then
			begin
			sellshort("S1") KCS shares next bar at Myflow stop;
			end;
		end;
	end;
//----------------------cover-------------------------------//
variables:
	cclineD(0), cclineK(0),
	EnPRSd(0), EnPRSk(0);
//cover
	cclineD = KbCnLp - 1 point;
	cclineK = KbCnHp + 1 point;
if MP <> MP[1] then
	begin
	EnPRSd = Myfhigh[1] ;
	EnPRSk = Myflow[1] ;
	end;
if MP > 0 then
	begin
	//sell ("zsPD1") all shares next bar at EnPRSd - TSxx stop;
	sell ("RzsPD1") all shares next bar at entryprice - TSxx stop;
	sell ("PD1") all shares next bar at cclineD stop;
	//sell ("PD2") all shares next bar at MA1[1] - 1 point stop; 
	end;
if MP < 0 then
	begin
	//buytocover ("zsPK1") all shares next bar at EnPRSk + TSxx stop;
	buytocover ("RzsPK1") all shares next bar at entryprice + TSxx stop;
	buytocover ("PK1") all shares next bar at cclineK stop;
	//buytocover ("PK2") all shares next bar at MA1[1] + 1 point stop;
	end;
//print(date," ",time," ",entryprice," ",CurrentContracts," ",TSx," ",TSxx); 



