ALTER TABLE scheduling_code 
ADD COLUMN time_off_code TINYINT(4) NOT NULL DEFAULT 0 AFTER priority_number;