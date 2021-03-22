import { Pipe, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SchedulingCode } from '../../system-admin/models/scheduling-code.model';
import { AgentScheduleGridResponse } from '../models/agent-schedule-grid-response.model';
import { ScheduleManagerAgentChartResponse, ScheduleManagerGridChartDisplay } from '../models/schedule-manager-chart.model';

@Pipe({
  name: 'getIconForSchedulingGrid',
  pure: true
})

export class GetIconSchedulingGridPipe implements PipeTransform {

  icon: string;
  actCode: actCode;

  public transform(week: number, gridTime: any, selectedGrid: AgentScheduleGridResponse, schedulingCodes: SchedulingCode[], updated): any {

      const weekData = selectedGrid?.agentScheduleCharts.find(x => x.day === +week);
      // const weekDataPrevDay = selectedGrid?.agentScheduleCharts.find(x => x.day === +(week)-1);
      
      if (weekData) {
        const currentDayData = weekData?.charts?.find(x => (this.convertToDateFormat(gridTime) >= this.convertToDateFormat(x?.startTime) &&
          this.convertToDateFormat(gridTime) < this.convertToDateFormat(x?.endTime)));

        // const nextDayOverlap = weekData?.charts.find(x => 
        //   ((this.convertToDateFormat(gridTime) >= this.convertToDateFormat(x?.startTime)) && 
        //   (this.convertToDateFormat(x?.startTime) > this.convertToDateFormat(x?.endTime)))
        // );

        if (currentDayData) {
          const code = schedulingCodes.find(x => x.id === currentDayData.schedulingCodeId);
          this.icon = code?.icon?.value;
          // this.actCode = new actCode;
          // this.actCode.description = code?.description;
          // this.actCode.icon = this.unifiedToNative(this.icon);
          // this.actCode.startTime = weekTimeData.startTime;
          // this.actCode.endTime = weekTimeData.endTime;

          return code ? this.unifiedToNative(code?.icon?.value) : '';
        }
      //   else if (nextDayOverlap){
      //     const code = schedulingCodes.find(x => x.id === nextDayOverlap.schedulingCodeId);
      //     this.icon = code?.icon?.value;
      //     // this.actCode = new actCode;
      //     // this.actCode.description = code?.description;
      //     // this.actCode.icon = this.unifiedToNative(this.icon);
      //     // this.actCode.startTime = weekTimeData.startTime;
      //     // this.actCode.endTime = weekTimeData.endTime;

      //     return code ? this.unifiedToNative(code?.icon?.value) : '';
      //   }

      // }

      // if(weekDataPrevDay){
      //   const prevDayData = weekDataPrevDay?.charts.find(x => 
      //     ((this.convertToDateFormat(gridTime) <= this.convertToDateFormat(x?.endTime)) && 
      //     (this.convertToDateFormat(x?.startTime) > this.convertToDateFormat(x?.endTime)))
      //   );

      //     if (prevDayData) {
      //       const code = schedulingCodes.find(x => x.id === prevDayData.schedulingCodeId);
      //       this.icon = code?.icon?.value;
      //       // this.actCode = new actCode;
      //       // this.actCode.description = code?.description;
      //       // this.actCode.icon = this.unifiedToNative(this.icon);
      //       // this.actCode.startTime = weekTimeData.startTime;
      //       // this.actCode.endTime = weekTimeData.endTime;
  
      //       return code ? this.unifiedToNative(code?.icon?.value) : '';
      //     }
      // }

      // if(this.actCode){
      //   return this.actCode;
      }
      // return;  
      return '';
  }

  private convertToDateFormat(time: string) {
    if (time) {
      const count = time.split(' ')[1].toLowerCase() === 'pm' ? 12 : undefined;
      if (count) {
        time = (+time.split(':')[0] + 12) + ':' + time.split(':')[1].split(' ')[0];
      } else {
        time = time.split(':')[0] + ':' + time.split(':')[1].split(' ')[0];
      }

      return time;
    }
  }

  getIconFromSelectedGrid(week: number, openTime: any, selectedGrid: AgentScheduleGridResponse,  schedulingCodes: SchedulingCode[]) {
    const weekData = selectedGrid?.agentScheduleCharts.find(x => x.day === +week);
    if (weekData) {
      const weekTimeData = weekData?.charts?.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
      }
    }

    return '';
  }    


  unifiedToNative(unified: string) {
      if (unified) {
        const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
        return String.fromCodePoint(...codePoints);
      }
    }

}

export class actCode {
  description: string;
  icon: any;
  endTime: string;
  startTime: string;
}

