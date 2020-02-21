//ZwF_POC，平仓数量（Number of closing positions）计算
inputs: FM(numericsimple); //持仓份额（分母）
inputs: FZ(numericsimple); //平仓份额（分子）
inputs: minPCS(numericsimple); //最小平仓量
inputs: LCS(numericsimple); //最小留仓量
inputs: POC(numericref); //平仓量
variables: MP(0);
variables: CTT(0);//初始持仓量
variables: CTTx(0);//实时持仓量
variables: PCSx(0);//理论平仓量
MP = marketposition;
CTTx = CurrentContracts;
if MP > 0 and MP[1] <> 1 then //首次开多单时
	CTT = CurrentContracts;
if MP < 0 and MP[1] <>-1 then //首次开空单时
	CTT = CurrentContracts;
if CTT > 0 then
	PCSx = Round(CTT/FM*FZ,0);
if MP <> 0 then //有持仓的情况下
	begin
	if CTTx > LCS then //实时持仓量大于最小留仓量
		begin
		if PCSx <= minPCS then //当理论平仓量小于最小平仓量
			begin
			if minPCS < CTTx then //当最小平仓量小于实时持仓量(*有多余仓位)
				begin
				if CTTx - LCS > minPCS then //当实时持仓量-最小留仓量>最小平仓量(*能够保持最低留仓量)
					begin
					POC = minPCS; //平仓量=最小平仓量
					end
				else
					begin
					POC = CTTx - LCS ; //平仓量=实时持仓量-最小留仓量（倒推平仓量）
					end;
				end
			else //(*无多余仓位)
				begin
				POC = CTTx - LCS; //平仓量=实时持仓量-最小留仓量（倒推平仓量）
				end;
			end
		else
			begin
			if PCSx < CTTx then //当理论平仓量小于实时持仓量(*有多余仓位)
				begin
				if CTTx - LCS > PCSx then  //当实时持仓量-最小留仓量>理论平仓量(*能够保持最低留仓量)
					begin
					POC = PCSx; //平仓量=理论平仓量
					end
				else
					begin
					POC = CTTx - LCS;  //平仓量=实时持仓量-最小留仓量（倒推平仓量）
					end;
				end
			else //当理论平仓量小于实时持仓量(*无多余仓位)
				begin
				POC = CTTx - LCS;  //平仓量=实时持仓量-最小留仓量（倒推平仓量）
				end;
			end;
		end
	else
		begin
		POC = 0;
		end;
	end;
	
