[IntrabarOrderGeneration = False]
//----------------------Index-------------------------------//
inputs:
	Price(C),BLth(50);
variables:
	MA1(0), NumD(2),BollUp(0), BollDn(0);
//BBand
value1 = zwBB(Price,BLth,NumD,MA1,BollUp,BollDn);
//----------------------EA Initialize-------------------------------//
variables:
	var0D(True), var0K(True);
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
//MaxR = BollUp - BollDn;
KCS = zzPSM(InCap,BSx,MaxCap,MinCap,KCX,BS,KcSlAmt,MaxR,InCapX);
//entry&add
if KCS > 0 then
	begin
	if MP <> MP[1] then
		TSxx = InCapX*BS*0.1/KCS/bigpointvalue;
	//Myfhigh = BollUp[1] + 1 point ;
	//Myflow = BollDn[1] - 1 point ;
	condition1 = HighD(0) <> LowD(0); //All
	//condition2 = BollUp[1] > BollUp[2] and H < var1D and MA1[1] > MA1[2]; //D
	//condition3 = BollDn[1] < BollDn[2] and L > var1K and MA1[1] < MA1[2]; //K
	condition2 = BollUp > BollUp[1] and MA1 > MA1[1]; //D
	condition3 = BollDn < BollDn[1] and MA1 < MA1[1];  //K
	if condition1 then
		begin
		if condition2 and Price crosses above BollUp then//
			begin
			buy("B1") KCS shares next bar at market;
			end;
		if condition3 and Price crosses below BollDn then//
			begin
			sellshort("S1") KCS shares next bar at market;
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
	if Price crosses below MA1 then
		begin
		sell ("PD") all shares next bar at market;
		end;
	sell ("RzsPD1") all shares next bar at entryprice - TSxx stop;
	sell ("zsPD") all shares next bar at cclineD stop;
	end;
if MP < 0 then
	begin
	if Price crosses above MA1 then
		begin
		buytocover ("PK") all shares next bar at market;
		end;
	buytocover ("RzsPK1") all shares next bar at entryprice + TSxx stop;
	buytocover ("zsPK") all shares next bar at cclineK stop;
	end;

