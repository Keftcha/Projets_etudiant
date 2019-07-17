-- SetupConfig: {"Requires":["CK.sZoneCreate"]}
--
create procedure IPR.sCreateTimePeriod
(
    @ActorId INT,
    @BegDate DATETIME2,
    @EndDate DATETIME2,
    @Kind CHAR,
    @TimePeriodIdResult INT OUTPUT,
    @ParentId INT = 0
)
as
begin
    --[beginsp]
    --<PreCreate />


              EXEC CK.sZoneCreate @ActorId, @TimePeriodIdResult OUTPUT, @ParentId;
              insert into IPR.tTimePeriod (TimePeriodId, BegDate, EndDate, Kind) VALUES (@TimePeriodIdResult, @BegDate, @EndDate, @Kind);


    --<PostCreate revert />

    --[endsp]
end

