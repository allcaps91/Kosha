{
program adds a alias to the BDE configuration file

 parameters:
   0: programname + path (standard parameter of OS)
   1: Name of alias
      if begins with '-' then delete first if exist
                         else do nothing if exist
   2: path to data directory
   3: BDE driver name
}

program AddAlias;

uses
  Windows, SysUtils, BDE;

type
  FError = array[0..DBIMAXMSGLEN+1] of Char;

var
  GAlias:    string  = 'New';
  GDriver:   string = szPARADOX;
  GAliasDir: string;
  FParams:   string;
  FDrvName:  string;
  FDelete:   boolean;
  i:         integer;

function StrToOem(const AnsiStr: string): string;
begin
  SetLength(Result, Length(AnsiStr));
  if Length(Result) > 0 then
    CharToOem(PChar(AnsiStr), PChar(Result));
end;


{----------------------------------------------------------------------------------------}
begin
  for i := 1 to ParamCount do
  begin
    case i of
      1: GAlias    := ParamStr(1);
      2: GAliasDir := ParamStr(2);
      3: GDriver   := ParamStr(3);
    end;
  end;

  //default aliasdir
  if GAliasDir = '' then GAliasDir := ExtractFilePath(ParamStr(0))+'Data';

  //should delete alias first? separate alias name
  if GAlias[1] = '-' then
  begin
    FDelete := True;
    GAlias := Copy(GAlias, 1, Length(GAlias));
  end else FDelete := False;

  //set parameters for the new alias
  FParams := Format('%s:"%s"',  [szCFGDBPATH, GAliasDir]) +
             Format(';%s:"%s"', [szCFGDBDEFAULTDRIVER, szPARADOX]) +
             Format(';%s:"%s"', [szCFGDBENABLEBCD, szCFGFALSE]);

  //set the driver name
  if (CompareText(GDriver, szCFGDBSTANDARD) = 0)
  then FDrvName := szPARADOX
  else FDrvName := GDriver;

  DbiInit(nil);
  try
    if FDelete then
      try
        DbiDeleteAlias(nil, PChar(GAlias));
      except
      end;

    try
      DbiAddAlias(nil, PChar(StrToOem(GAlias)),
                       PChar(StrToOem(FDrvName)),
                       PChar(FParams), True);
      DbiCfgSave(nil, nil, True);
    except
    end;

  finally
    DbiExit();
  end;

end.




