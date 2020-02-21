[IntrabarOrderGeneration = False]
//------------------指标计算------------------//
//------------------布林指标------------------//
inputs:
	BBLength(50); 
variables:
	MAbb(0), BollUp(0), BollDn(0);
//BBand
value1 = Zw_F_BB(Close,BBLength,2,MAbb,BollUp,BollDn);

variables:
	MaxR(0);
MaxR = BollUp - BollDn;
//----------------------EA Initialize-------------------------------//
variables:
	var0D(True), var0K(True),
	HHH(High), LLL(Low);
if Close crosses above BollUp then//
	begin
	HHH = High;
	var0D = True;
	end
else
	begin
	condition1 = var0D and High >= HHH + 1 point;
	if condition1 then
		begin
		var0D = False;
		end;
	end;
if Close crosses below BollDn then//
	begin
	LLL = Low;
	var0K = True;
	end
else
	begin
	condition1 = var0K and High <= LLL - 1 point;
	if condition1 then
		begin
		var0K = False;
		end;
	end;

	
//----------------------Open positions-------------------------------//
Input:	
	InCap(500000),MaxCap(500000),MinCap(500000),
	BSx(0.7),KCX(4), BS(1.5), KcSlAmt(0);
variables:
	MP(0), KCS(0), InCapX(0),TSxx(0),
	Myfhigh(H), Myflow(L);
//Initialize	
MP = marketposition;
KCS = zzPSM(InCap,BSx,MaxCap,MinCap,KCX,BS,KcSlAmt,MaxR,InCapX);
//entry&add
if KCS > 0 then
	begin
	if MP <> MP[1] then
		TSxx = InCapX*BS*0.01/KCS/bigpointvalue;
	Myfhigh = HHH + 1 point ;
	Myflow = LLL - 1 point ;
	condition1 = HighD(0) <> LowD(0); //All
	//condition2 = BollUp[1] > BollUp[2] and H < var1D and MA1[1] > MA1[2]; //D
	//condition3 = BollDn[1] < BollDn[2] and L > var1K and MA1[1] < MA1[2]; //K
	condition2 = var0D;// and BollUp > BollUp[1] and MA1 > MA1[1]; //D
	condition3 = var0K;// and BollDn < BollDn[1] and MA1 < MA1[1];  //K
	if condition1 then
		begin
		if condition2 then//
			begin
			buy("B1") KCS shares next bar at Myfhigh stop;
			end;
		if condition3 then//
			begin
			sellshort("S1") KCS shares next bar at Myflow stop;
			var0K = False;
			end;
		end;
	end ;
//----------------------cover-------------------------------//
variables:
	cclineD(0), cclineK(0);
//cover
	cclineD = BollDn[1] - 1 point;
	cclineK = BollUp[1] + 1 point;
if MP > 0 then
	begin
	{if Close crosses below MA1 then
		begin
		sell ("PD") all shares next bar at market;
		end;}
	sell ("RzsPD1") all shares next bar at entryprice - TSxx stop;
	sell ("zsPD") all shares next bar at cclineD stop;
	end;
if MP < 0 then
	begin
	{if Close crosses above MA1 then
		begin
		buytocover ("PK") all shares next bar at market;
		end;}
	buytocover ("RzsPK1") all shares next bar at entryprice + TSxx stop;
	buytocover ("zsPK") all shares next bar at cclineK stop;
	end;
