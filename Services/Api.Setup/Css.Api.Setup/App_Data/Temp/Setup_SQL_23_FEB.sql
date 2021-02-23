/* Please append the additional db scripts here */
ALTER TABLE agent_scheduling_group ADD COLUMN estart_provision TINYINT NOT NULL DEFAULT 0 AFTER is_deleted;

