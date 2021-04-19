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
      
      if (weekData) {
        const currentDayData = weekData?.charts?.find(x => (this.convertToDateFormat(gridTime) >= this.convertToDateFormat(x?.startTime) &&
          this.convertToDateFormat(gridTime) < this.convertToDateFormat(x?.endTime)));

        if (currentDayData) {
          const code = schedulingCodes.find(x => x.id === currentDayData.schedulingCodeId);
          this.icon = code?.icon?.value;

          return code ? this.unifiedToNative(code?.icon?.value) : '';
        }

      return '';
    }
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

