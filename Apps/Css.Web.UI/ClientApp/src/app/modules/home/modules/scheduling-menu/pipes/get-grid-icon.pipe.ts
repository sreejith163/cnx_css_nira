import { Pipe, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SchedulingCode } from '../../system-admin/models/scheduling-code.model';
import { ScheduleManagerChart, ScheduleManagerAgentChartResponse, ScheduleManagerGridChartDisplay, ScheduleManagerChartDisplay } from '../models/schedule-manager-chart.model';

@Pipe({
  name: 'getIconForGrid',
  pure: true
})

export class GetIconForGridPipe implements PipeTransform {

    icon: string;
    actCode: actCode;
    public transform(updatedTimeStamp, schedulingCodes:SchedulingCode[],  date: string, el: ScheduleManagerAgentChartResponse): any {
        if(el.charts){
            let weekTimeData = el.charts?.find(x => this.getTimeStamp(date) >= this.getTimeStamp(x?.startDateTime) &&
            this.getTimeStamp(date) < this.getTimeStamp(x?.endDateTime));
        
            if(weekTimeData){
                const code = schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
                this.icon = code?.icon?.value;
                this.actCode = new actCode;
                this.actCode.description = code?.description;
                this.actCode.icon = this.unifiedToNative(this.icon);
                return this.actCode;                
            }
            return '';        
        }        
    }

    changeToUTCDate(date){
      return new Date(new Date(date).toString().slice(0, 24).concat(" GMT+0000"));
    }

    unifiedToNative(unified: string) {
        if (unified) {
          const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
          return String.fromCodePoint(...codePoints);
        }
      }

      getTimeStamp(date: any){
        if(date){
          date = new Date(new Date(date).toString().slice(0, 24).concat(" GMT+0000"))?.getTime()
        }
    
        return date;
      }
}

export class actCode {
    description: string;
    icon: any;
}
