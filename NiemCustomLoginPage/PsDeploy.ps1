Install-SPFeature "lmd.NIEM.FarmSolution_NIEMSiteSettings"
Install-SPFeature "NIEM - Anonymous Discussions"
Install-SPFeature "NIEM - Group Member Identification"
Install-SPFeature "NIEM - Item level permission"
Install-SPFeature "NIEM - Layouts Page Mapping"
Install-SPFeature "lmd.NIEM.FarmSolution_NiemCustomWebparts"
Enable-SPFeature "lmd.NIEM.FarmSolution_NIEMSiteSettings" -Url http://niem-3.cld.sr:8001
Enable-SPFeature "NIEM - Anonymous Discussions" -Url http://niem-3.cld.sr:8001
Enable-SPFeature "NIEM - Group Member Identification" -Url http://niem-3.cld.sr:8001
Enable-SPFeature "NIEM - Item level permission" -Url http://niem-3.cld.sr:8001
Enable-SPFeature "NIEM - Layouts Page Mapping" -Url http://niem-3.cld.sr:8001
Enable-SPFeature "lmd.NIEM.FarmSolution_NiemCustomWebparts" -Url http://niem-3.cld.sr:8001

