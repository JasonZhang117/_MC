inputs:
	PriceH(numericseries),PriceL(numericseries), 
	KbCnLen(numericsimple),
	KbCnH(numericref),KbCnL(numericref);

if zwTL then
	begin
	KbCnH = HighestFC(PriceH, KbCnLen);
	KbCnL = LowestFC(PriceL, KbCnLen);
	Zw_F_KBC= 1;
	end;
