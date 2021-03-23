import { Pipe, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SchedulingCode } from '../../system-admin/models/scheduling-code.model';
import { ActivityLogsManagerChart } from '../models/activity-log-schedule-manager.model';
import { ActivityLogsChart } from '../models/activity-logs-chart.model';
import { ScheduleManagerAgentChartResponse, ScheduleManagerGridChartDisplay } from '../models/schedule-manager-chart.model';

@Pipe({
  name: 'scheduleManagerActivityLogsIcon',
  pure: true
})

export class GetIconForScheduleManagerActivityLogsPipe implements PipeTransform {

    icon: string;
    actCode: actCode;

    public transform(id, activityLogsCharts: ActivityLogsManagerChart[], schedulingCodes:SchedulingCode[],  date: string): any {

            if(activityLogsCharts?.length > 0){
                const chart = activityLogsCharts.find(x => x?.id === +id);
                let weekTimeData = chart?.agentScheduleChart?.charts.find(x => this.getTimeStamp(date) >= this.getTimeStamp(x?.startDateTime) &&
                this.getTimeStamp(date) < this.getTimeStamp(x?.endDateTime));
                if(weekTimeData){
                    const code = schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
                    this.icon = code?.icon?.value;
                    this.actCode = new actCode;
                    this.actCode.description = code?.description;
                    this.actCode.icon = this.unifiedToNative(this.icon);
                }
            }

        if(this.actCode){
            return this.actCode;
        }
        return;        
    }

    changeToUTCDate(date){
        return new Date(new Date(date).toString().replace("GMT+0800","GMT+0000"));
    }

    unifiedToNative(unified: string) {
        if (unified) {
          const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
          return String.fromCodePoint(...codePoints);
        }
      }

      getTimeStamp(date: any){
        if(date){
          date = new Date(date)?.getTime()
        }
    
        return date;
      }
}

export class actCode {
    description: string;
    icon: any;
}
