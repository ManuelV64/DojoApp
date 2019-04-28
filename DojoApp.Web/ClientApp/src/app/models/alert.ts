import { IAlert } from '../interfaces/i-alert';

export class Alert implements IAlert {
  constructor(obj?: any) {
    this.header  = obj && obj.header  || null;
    this.message = obj && obj.message || null;
    this.status  = obj && obj.status  || 0;
  }

  header: string;
  message: string;
  status: number;

  public AlertMessage(): IAlert {
    if (this.status !== 0) {
      return { header: this.header , message: `Error ${this.status} -- ${this.message}` };
    }

    return  this;
  }
}
