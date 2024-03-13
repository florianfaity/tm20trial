import {EDifficulty} from "../../web-api-client";
import {Pipe, PipeTransform} from "@angular/core";

@Pipe({ name: 'difficulty' })
export class DifficultyPipe implements PipeTransform {
  transform(value?: EDifficulty, ...args: any[]): string {
    switch (value) {
      case EDifficulty.Easy:
        return 'Easy';
      case EDifficulty.Intermediate:
        return 'Intermediate';
      case EDifficulty.Advanced:
        return 'Advanced';
      case EDifficulty.Hard:
        return 'Hard';
      case EDifficulty.Expert:
        return 'Expert';
      case EDifficulty.Insane:
        return 'Insane';
      case EDifficulty.None:
      default:
        return 'Easy';
    }
  }
}
