DECLARE @CKTraitContextIdResult INT

-- I is for PI ( as in the type of the project)
EXEC CK.sCKTraitContextRegister 1, 'I', ';', @CKTraitContextIdResult OUTPUT;

-- H is for PFH ( as in the type of the project)
EXEC CK.sCKTraitContextRegister 1, 'H', ';', @CKTraitContextIdResult OUTPUT;

