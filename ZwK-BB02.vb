[IntrabarOrderGeneration = False]
//----------------------Index-------------------------------//
inputs:
	Price(C),BLth(50),NumD(2);
variables:
	MA1(0), BollUp(0), BollDn(0);
//BBand
value1 = zwBB(Price,BLth,NumD,MA1,BollUp,BollDn);
//----------------------Open positions-------------------------------//
Input:	
	InCap(100000), KCX(4), BS(0.01), KcSlAmt(1), psAlen(20);
variables:
	MP(0), KCS(0),
	Myfhigh(H), Myflow(L);
//Initialize	
MP = marketposition;
KCS = zwPSM(InCap,KCX,BS,KcSlAmt,psAlen);
//entry&add
if KCS > 0 then
	begin
	Myfhigh = BollUp[1] + 1 point ;
	Myflow = BollDn[1] - 1 point ;
	condition1 = High <> Low; //All
	//condition2 = BollUp[1] > BollUp[2] and H < var1D and MA1[1] > MA1[2]; //D
	//condition3 = BollDn[1] < BollDn[2] and L > var1K and MA1[1] < MA1[2]; //K
	condition2 = BollUp[1] > BollUp[2] and MA1[1] > MA1[2]; //D
	condition3 = BollDn[1] < BollDn[2] and MA1[1] < MA1[2];  //K
	if condition1 then
		begin
		if condition2 then//
			begin
			buy("B1") KCS shares next bar at Myfhigh stop ;
			end;
		if condition3 then//
			begin
			sellshort("S1") KCS shares next bar at Myflow stop;
			end;
		end;
	end ;
//----------------------cover-------------------------------//
variables:
	cclineD(0), cclineK(0);
//cover
	cclineD = MA1[1] - 1 point;
	cclineK = MA1[1] + 1 point;
if MP > 0 then
	begin
	sell ("PD") all shares next bar at cclineD stop;
	end;
if MP < 0 then
	begin
	buytocover ("PK") all shares next bar at cclineK stop;
	end;

