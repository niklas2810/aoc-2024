namespace AOC2024.Cli.Solutions;

public class Day09 : DayBase
{
    public override string Name => "Disk Fragmenter";

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // The input is a single long line.
        var line = input.Single();

        // The line is a sequence of digits. Every even index (first, third, fifth, etc.) is a file block size, and every odd index is an empty block size.
        var blocks = line.ToCharArray().Select(x => x - '0').ToArray();
        var fileBlocks = blocks.Where((_, i) => i % 2 == 0).ToArray();
        var emptyBlocks = blocks.Where((_, i) => i % 2 == 1).ToArray();

        // Calculate total block size.
        var totalBlocks = fileBlocks.Sum() + emptyBlocks.Sum(); 


        // We need to move file blocks one at a time from the end of the disk to the leftmost free space block.
        // We only care for the final block arrangement therefore we do the following approach:
        // 1. Keep two indices for file blocks: Front and Back. Front starts at 0 and Back starts at the last index.
        // 2. If we are inside a file block, move the Front index to the right and keep the file where it is.
        // 3. If we are inside an empty block, move the Back index to the left and move the file block from the back to the empty block.
        // 4. Print out the index in fileBlocks from where we took the file block in each iteration.
        // 5. Repeat until the two indices meet.
        var front = 0;
        var back = fileBlocks.Length - 1;
        var backRemainder = 0; // If we can only place a part of the back file block in the empty block, we keep the remainder here.
        var printed = 0;

        var checksum = 0L;
        while(true) {
            if(front >= back)
            {
                if(backRemainder == 0)
                  backRemainder = fileBlocks[back];
                for (var i = 0; i < backRemainder; i++) {
                    checksum += back * printed;
                    printed++;
                }
            
                // We are done.
                break;
            }
            // First print the file block x times.
            var currentBlock = fileBlocks[front];
            for (var i = 0; i < currentBlock; i++) {
                checksum += front * printed;
                printed++;
            }
            // Now, fill the empty blocks after the file with content from the back.
            // Keep in mind that the empty block may not be enough to fill the file block, or may be enough to fit multiple file blocks from the back.
            if(front == emptyBlocks.Length)
                break;
            var emptyBlock = emptyBlocks[front];
            while(emptyBlock > 0) {
                if(front == back) {
                    // We are done.
                    break;
                }
                var backBlock = fileBlocks[back];
                if(backRemainder > 0)
                    backBlock = backRemainder;
                if(emptyBlock >= backBlock) {
                    // We can fit the whole back block in the empty block.
                    for (var i = 0; i < backBlock; i++) {
                        checksum += back * printed;
                        printed++;
                    }
                    emptyBlock -= backBlock;
                    backRemainder = 0;
                    back--;
                } else {
                    // We can only fit a part of the back block in the empty block.
                    for (var i = 0; i < emptyBlock; i++) {
                        checksum += back * printed;
                        printed++;
                    }
                    backRemainder = backBlock - emptyBlock;
                    emptyBlock = 0;
                }
            }
            ++front;
        }
        return Task.FromResult(checksum);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // The input is a single long line.
        var line = input.Single();

        // The line is a sequence of digits. Every even index (first, third, fifth, etc.) is a file block size, and every odd index is an empty block size.
        var blocksInput = line.ToCharArray().Select(x => x - '0').ToArray();
        var fileBlocks = blocksInput.Where((_, i) => i % 2 == 0).ToArray();
        var emptyBlocks = blocksInput.Where((_, i) => i % 2 == 1).ToArray();

        // Calculate total block size.
        var totalBlocks = fileBlocks.Sum() + emptyBlocks.Sum(); 
        var blocks = new int[totalBlocks];

        var gi = 0;
        for(int i = 0; i < fileBlocks.Length; i++)
        {
            var fileBlock = fileBlocks[i];
            for(int j = 0; j < fileBlock; j++)
            {
                blocks[gi++] = i;
            }
            if(i == emptyBlocks.Length)
                break;
            var emptyBlock = emptyBlocks[i];
            for(int j = fileBlock; j < fileBlock + emptyBlock; j++)
            {
                blocks[gi++] = -1;
            }
        }


        for(int i = fileBlocks.Length-1; i > 0; i--) {
            // Find first and last occurrence of i in blocks.
            var start = Array.IndexOf(blocks, i);
            var end = Array.LastIndexOf(blocks, i);
            var length = end - start + 1;
            
            // Now, try to find an empty block (-1) that is at least as long as the file block.
            var emptyBlockStart = 0;
            while(true) {
                emptyBlockStart = Array.IndexOf(blocks, -1, emptyBlockStart);
                if(emptyBlockStart == -1 || emptyBlockStart > start)
                    break;
                // Calculate the length of the found empty block. This means where the next number != -1
                var emptyBlockLength = 1;
                while(blocks[emptyBlockStart + emptyBlockLength] == -1)
                    emptyBlockLength++;
                if(emptyBlockLength < length)
                {
                    emptyBlockStart += emptyBlockLength;
                    continue;   
                }
                // We found a suitable empty block. Now, move the file block to the empty block.
                for(int j = 0; j < length; j++)
                {
                    blocks[emptyBlockStart + j] = i;
                    blocks[start + j] = -1;
                }
                break;
            }
        }


        // Calculate the checksum as above.
        var checksum = blocks.Select((x, i) => x < 0 ? 0L : (long)(x * i)).Sum();

        return Task.FromResult(checksum);
    }
}
