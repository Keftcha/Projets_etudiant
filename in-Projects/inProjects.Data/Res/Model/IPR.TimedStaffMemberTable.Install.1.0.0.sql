CREATE TABLE IPR.tTimedStaffMember
(
    TimedStaffMemberId INT NOT NULL,

    CONSTRAINT PK_tTimedStaffMember_TimedStaffMemberId PRIMARY KEY(TimedStaffMemberId),
    CONSTRAINT FK_tTimedStaffMember_TimedStaffMemberId FOREIGN KEY(TimedStaffMemberId) REFERENCES IPR.tTimedUser(TimedUserId),
 
)

